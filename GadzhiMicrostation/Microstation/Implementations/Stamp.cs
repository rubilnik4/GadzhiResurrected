using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Штамп
    /// </summary>
    public class Stamp : IStamp
    {
        /// <summary>
        /// Экземпляр ячейки Microstation определяющей штамп
        /// </summary>
        private readonly CellElement _stampCellElement;

        /// <summary>
        /// Доступные поля в Штампе
        /// </summary>
        private IDictionary<string, IElementMicrostation> _stampFields;

        public Stamp(CellElement stampCellElement)
        {
            _stampCellElement = stampCellElement;
            _stampFields = new Dictionary<string, IElementMicrostation>();

            FillDataFields();
        }

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        private void FillDataFields()
        {
            if (_stampCellElement != null)
            {
                ElementEnumerator elementEnumerator = _stampCellElement.GetSubElements();

                while (elementEnumerator.MoveNext())
                {
                    var element = (Element)elementEnumerator.Current;
                    if (element.IsTextElement || element.IsTextNodeElement)
                    {
                        string controlName = element.GetAttributeControlName();

                        if (StampElement.ContainControlName(controlName))
                        {
                            AddElementToDictionary(controlName, element);
                        }
                        else
                        {
                            FillFormat(element);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Найти и заполнить поле формат
        /// </summary>
        private void FillFormat(Element element)
        {
            if (element.IsTextElement)
            {
                string textOfElement = element.AsTextElement.Text;
                if (StampMain.IsFormatField(textOfElement))
                {
                    AddElementToDictionary(StampMain.Format, element);
                }
            }
        }

        /// <summary>
        /// Добавить элемент в словарь
        /// </summary>       
        private void AddElementToDictionary(string controlName, Element element)
        {
            if (element.IsTextElement)
            {
                _stampFields.Add(controlName,
                                 new TextElementMicrostation(element.AsTextElement));
            }
            else if (element.IsTextNodeElement)
            {
                _stampFields.Add(controlName,
                                 new TextNodeElementMicrostation(element.AsTextNodeElement));
            }
            else
            {
                _stampFields.Add(controlName,
                                 new ElementMicrostation(element));
            }

        }

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public void CompressFieldsRanges()
        {
            foreach (var element in _stampFields.Values)
            {
                if (element.IsTextElementMicrostation())
                {

                }
            }
        }
    }
}
