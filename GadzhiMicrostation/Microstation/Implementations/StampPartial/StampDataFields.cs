using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
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
        private IDictionary<string, IElementMicrostation> _stampFields;

        /// <summary>
        /// Инициализировать данные
        /// </summary>
        private void InitialStampDataFields()
        {
            _stampFields = new Dictionary<string, IElementMicrostation>();

            FillDataFields();
        }

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        private void FillDataFields()
        {
            var subTextElements = GetSubElements()?.Where(subElement => subElement.ElementType == ElementMicrostationType.TextElement ||
                                                                        subElement.ElementType == ElementMicrostationType.TextNodeElement);
            foreach (var subElement in subTextElements)
            {
                if (StampElement.ContainControlName(subElement.AttributeControlName))
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
        private void FillFormat(IElementMicrostation element)
        {
            if (element.IsTextElementMicrostation)
            {
                var textElement = element.AsTextElementMicrostation;
                if (StampMain.IsFormatField(textElement.Text))
                {
                    textElement.AttributeControlName = StampMain.Format.Name;
                    AddElementToDictionary(textElement);
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
                return _stampFields[fieldSearch];
            }

            return null;
        }

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        public IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<string> fieldSearch,
                                                                           ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
                                                 fieldSearch?.Where(_stampFields.ContainsKey).
                                                 Select(fieldName => _stampFields[fieldName]).
                                                 Where(fieldName => elementMicrostationType == ElementMicrostationType.Element ||
                                                                    fieldName.ElementType == elementMicrostationType);

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public void CompressFieldsRanges()
        {
            foreach (var element in _stampFields.Values)
            {
                switch (element.ElementType)
                {
                    case ElementMicrostationType.TextElement:
                        element.AsTextElementMicrostation.CompressRange();                      
                        break;
                    case ElementMicrostationType.TextNodeElement:
                        if (element.AsTextNodeElementMicrostation.CompressRange())
                        {
                            FindAndChangeSubElement(element.Id);                           
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Добавить элемент в словарь
        /// </summary>       
        private void AddElementToDictionary(IElementMicrostation element)
        {
            StampBaseField stampBaseField = StampElement.GetBaseParametersByControlName(element.AttributeControlName);

            if (element.IsTextElementMicrostation)
            {
                var textElement = element.AsTextElementMicrostation;
                textElement.IsNeedCompress = stampBaseField.IsNeedCompress;
                textElement.IsVertical = stampBaseField.IsVertical;

                _stampFields.Add(element.AttributeControlName, textElement);
            }
            else if (element.IsTextNodeElementMicrostation)
            {
                var textnodeElement = element.AsTextNodeElementMicrostation;
                textnodeElement.IsNeedCompress = stampBaseField.IsNeedCompress;
                textnodeElement.IsVertical = stampBaseField.IsVertical;

                _stampFields.Add(element.AttributeControlName, textnodeElement);
            }

        }
    }
}
