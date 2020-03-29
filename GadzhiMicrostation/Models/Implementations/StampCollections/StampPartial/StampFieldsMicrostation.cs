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
                                                 Where(fieldName => elementMicrostationType == ElementMicrostationType.Element ||
                                                                    fieldName.ElementType == elementMicrostationType).
                                                 Join(fieldsSearch,
                                                      subElement => subElement.AttributeControlName,
                                                      fieldSearch => fieldSearch,
                                                      (subElement, fieldSearch) => subElement);

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
        public override void CompressFieldsRanges()
        {
            foreach (var element in StampCellElement.SubElements)
            {
                switch (element.ElementType)
                {
                    case ElementMicrostationType.TextElement:
                        element.AsTextElementMicrostation.CompressRange();
                        break;
                    case ElementMicrostationType.TextNodeElement:
                        if (element.AsTextNodeElementMicrostation.CompressRange())
                        {
                            StampCellElement.FindAndChangeSubElement(element);
                        }
                        break;
                }
            }
        }
    }
}
