using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

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
        private IEnumerable<IElementMicrostation> StampSubControls { get; }

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        private static IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<IElementMicrostation> cellSubElements,
                                                                                   IEnumerable<string> fieldsSearch,
                                                                                   ElementMicrostationType elementMicrostationType = ElementMicrostationType.Element) =>
                cellSubElements.
                Where(subElement => (elementMicrostationType == ElementMicrostationType.Element ||
                                     subElement.ElementType == elementMicrostationType) &&
                                     subElement is IRangeBaseElementMicrostation<IElementMicrostation>).
                Cast<IRangeBaseElementMicrostation<IElementMicrostation>>().
                Where(subElement => fieldsSearch?.Contains(subElement.AttributeControlName) == true).
                Select(subElement => subElement.Clone(StampFieldMain.IsControlVertical(subElement.AttributeControlName)));

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        protected IEnumerable<IElementMicrostation> FindElementsInStampControls(IEnumerable<string> fieldsSearch,
                                                                                ElementMicrostationType elementMicrostationType = ElementMicrostationType.Element) =>
            FindElementsInStampFields(StampSubControls, fieldsSearch, elementMicrostationType);

        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        public IStampFieldMicrostation GetFieldFromElements(IEnumerable<ITextElementMicrostation> elementsMicrostation,
                                                            HashSet<StampFieldBase> stampFields, StampFieldType stampFieldType) =>
                elementsMicrostation.Where(element => stampFields.
                                                      Select(field => field.Name).
                                                      Contains(element.AttributeControlName)).
                                     Select(field => new StampFieldMicrostation(field, stampFieldType)).
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
    }
}
