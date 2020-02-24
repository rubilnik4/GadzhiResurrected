using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Подкласс для работа с внутренними полями штампа
    /// </summary>
    public partial class Stamp : IStampDataFields
    {
        /// <summary>
        /// Доступные поля в Штампе
        /// </summary>
        private IDictionary<string, ElementMicrostationPair> _stampFields;

        /// <summary>
        /// Инициализировать данные
        /// </summary>
        private void InitialStampDataFields()
        {
            _stampFields = new Dictionary<string, ElementMicrostationPair>();

            FillDataFields();
        }

        /// <summary>
        /// Доступные поля в штампе в формате обертки
        /// </summary>
        private IDictionary<string, IElementMicrostation> _stampFieldsWrapper =>
            _stampFields.ToDictionary(pair => pair.Key,
                                      pair => pair.Value.ElementWrapper);

        /// <summary>
        /// Формат штампа
        /// </summary>
        public string PaperSize => StampMain.GetPaperSizeFromField(_stampFieldsWrapper[StampMain.PaperSize.Name]?.AsTextElementMicrostation?.Text) ??
                                   String.Empty;

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        private void FillDataFields()
        {
            var subTextElements = GetSubElements()?.Where(subElement => subElement.ElementWrapper.ElementType == ElementMicrostationType.TextElement ||
                                                                        subElement.ElementWrapper.ElementType == ElementMicrostationType.TextNodeElement);
            foreach (var subElement in subTextElements)
            {
                if (StampElement.ContainControlName(subElement.ElementWrapper.AttributeControlName))
                {
                    AddElementToDictionary(subElement);
                }
                else
                {
                    FillFormat(subElement);
                }
            }
        }

        /// <summary>
        /// Найти и заполнить поле формат
        /// </summary>
        private void FillFormat(ElementMicrostationPair elementPair)
        {
            if (elementPair.ElementWrapper.IsTextElementMicrostation)
            {
                var textElement = elementPair.ElementWrapper.AsTextElementMicrostation;
                if (StampMain.IsFormatField(textElement.Text))
                {
                    textElement.AttributeControlName = StampMain.PaperSize.Name;
                    AddElementToDictionary(elementPair);
                }
            }
        }

        /// <summary>
        /// Найти элемент в словаре штампа по ключам
        /// </summary>
        public IElementMicrostation FindElementInStampFields(string fieldSearch)
        {
            if (_stampFields.ContainsKey(fieldSearch))
            {
                return _stampFieldsWrapper[fieldSearch];
            }

            return null;
        }

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        public IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<string> fieldSearch,
                                                                           ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
                                                 fieldSearch?.Where(_stampFieldsWrapper.ContainsKey).
                                                 Select(fieldName => _stampFieldsWrapper[fieldName]).
                                                 Where(fieldName => elementMicrostationType == ElementMicrostationType.Element ||
                                                                    fieldName.ElementType == elementMicrostationType);

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public void CompressFieldsRanges()
        {
            foreach (var elementPair in _stampFields.Values)
            {
                switch (elementPair.ElementWrapper.ElementType)
                {
                    case ElementMicrostationType.TextElement:
                        elementPair.ElementWrapper.AsTextElementMicrostation.CompressRange();
                        break;
                    case ElementMicrostationType.TextNodeElement:
                        if (elementPair.ElementWrapper.AsTextNodeElementMicrostation.CompressRange())
                        {
                            FindAndChangeSubElement(elementPair.ElementOriginal);
                        }
                        break;
                }
            }

            double dd = CellElement.Range.High.X;
        }

        /// <summary>
        /// Добавить элемент в словарь
        /// </summary>       
        private void AddElementToDictionary(ElementMicrostationPair elementPair)
        {
            StampBaseField stampBaseField = StampElement.GetBaseParametersByControlName(elementPair.ElementWrapper.AttributeControlName);

            if (elementPair.ElementWrapper.IsTextElementMicrostation)
            {
                var textElement = elementPair.ElementWrapper.AsTextElementMicrostation;
                textElement.IsNeedCompress = stampBaseField.IsNeedCompress;
                textElement.IsVertical = stampBaseField.IsVertical;

                _stampFields.Add(elementPair.ElementWrapper.AttributeControlName, elementPair);
            }
            else if (elementPair.ElementWrapper.IsTextNodeElementMicrostation)
            {
                var textnodeElement = elementPair.ElementWrapper.AsTextNodeElementMicrostation;
                textnodeElement.IsNeedCompress = stampBaseField.IsNeedCompress;
                textnodeElement.IsVertical = stampBaseField.IsVertical;

                _stampFields.Add(elementPair.ElementWrapper.AttributeControlName, elementPair);
            }

        }
    }
}
