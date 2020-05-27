using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiMicrostation.Models.Implementations.StampCollections.Fields;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс для работа с внутренними полями штампа
    /// </summary>
    public abstract partial class StampMicrostation
    {
        /// <summary>
        /// Дочерние элементы штампа
        /// </summary>
        private IResultAppCollection<IElementMicrostation> _stampSubControls;

        /// <summary>
        /// Дочерние элементы штампа
        /// </summary>
        private IResultAppCollection<IElementMicrostation> StampSubControls =>
            _stampSubControls ??= FindElementsInFields<IElementMicrostation>(StampCellElement.SubElements, StampFieldElement.StampControlNames).
                                  Map(foundElements => new ResultAppCollection<IElementMicrostation>(foundElements));

        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        public IStampTextFieldMicrostation GetFieldFromElements(IEnumerable<ITextElementMicrostation> elementsMicrostation,
                                                            HashSet<StampFieldBase> stampFields, StampFieldType stampFieldType) =>
                elementsMicrostation.Where(element => stampFields.
                                                      Select(field => field.Name).
                                                      Contains(element.AttributeControlName)).
                                     Select(field => new StampTextFieldMicrostation(field, stampFieldType)).
                                     FirstOrDefault();

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override IEnumerable<bool> CompressFieldsRanges() =>
            StampCellElement.SubElements.Select(element =>
                element.ElementType switch
                {
                    ElementMicrostationType.TextElement => element.AsTextElementMicrostation.CompressRange(),
                    ElementMicrostationType.TextNodeElement => element.AsTextNodeElementMicrostation.CompressRange().
                        WhereOk(isCompressed => isCompressed, okFunc: isCompressed =>
                        {
                            StampCellElement.FindAndChangeSubElement(element);
                            return isCompressed;
                        }),
                    _ => false
                }
            );

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        protected IResultAppCollection<TElement> FindElementsInStamp<TElement>(IEnumerable<string> fieldsSearch, IErrorApplication errorNull)
            where TElement : IElementMicrostation =>
            StampSubControls.
                ResultValueOk(subElements => FindElementsInFields<TElement>(subElements, fieldsSearch)).
                ToResultCollection().
                ResultValueContinue(fields => fields.Count > 0,
                                    okFunc: fields => fields,
                                    badFunc: fields => errorNull).
                ToResultCollection();

        /// <summary>
        /// Найти элементы в словаре по ключам
        /// </summary>
        private static IEnumerable<TElement> FindElementsInFields<TElement>(IEnumerable<IElementMicrostation> cellSubElements,
                                                                            IEnumerable<string> fieldsSearch)
            where TElement : IElementMicrostation =>
            cellSubElements.
                Where(subElement => subElement is IRangeBaseElementMicrostation<TElement>).
                Cast<IRangeBaseElementMicrostation<TElement>>().
                Where(subElement => fieldsSearch?.Contains(subElement.AttributeControlName) == true).
                Select(subElement => subElement.Clone(StampFieldMain.IsControlVertical(subElement.AttributeControlName)));
    }
}
