using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<IElementMicrostation> cellSubElements,
                                                                            IEnumerable<string> fieldsSearch,
                                                                            ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
                cellSubElements?.
                Where(subElement => (elementMicrostationType == ElementMicrostationType.Element ||
                                     subElement.ElementType == elementMicrostationType) &&
                                     subElement is IRangeBaseElementMicrostation<IElementMicrostation>).
                Cast<IRangeBaseElementMicrostation<IElementMicrostation>>().
                Join(fieldsSearch,
                     subElement => subElement.AttributeControlName,
                     fieldSearch => fieldSearch,
                     (subElement, fieldSearch) => subElement).
                Select(subElement => subElement.Copy(StampFieldMain.IsControlVertical(subElement.AttributeControlName))).
                Cast<IElementMicrostation>();

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        protected IEnumerable<IElementMicrostation> FindElementsInStampControls(IEnumerable<string> fieldsSearch,
                                                                                ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
            FindElementsInStampFields(StampSubControls, fieldsSearch, elementMicrostationType);

        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        public IStampFieldMicrostation GetFieldFromElements(IEnumerable<ITextElementMicrostation> elementsMicrostation,
                                                            HashSet<StampFieldBase> stampFields, StampFieldType stampFieldType) =>
                elementsMicrostation?.Where(element => stampFields?.
                                                       Select(field => field.Name).
                                                       Contains(element.AttributeControlName) == true)?.
                                      Select(field => new StampFieldMicrostation(field, stampFieldType))?.
                                      FirstOrDefault();

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override Unit CompressFieldsRanges() =>
            StampCellElement.SubElements.Select(element =>
            {
                switch (element.ElementType)
                {
                    case ElementMicrostationType.TextElement:
                        return element.AsTextElementMicrostation.CompressRange();                      
                    case ElementMicrostationType.TextNodeElement:
                        return element.AsTextNodeElementMicrostation.CompressRange().
                               WhereOK(isCompressed => isCompressed,
                               okFunc: isCompressed => { StampCellElement.FindAndChangeSubElement(element); 
                                                         return isCompressed; });                       
                    default:
                        return false;
                }
              
            }).
            Map(_ => Unit.Value);       
    }
}
