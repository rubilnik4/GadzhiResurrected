using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enum;
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
                        //if (!String.IsNullOrEmpty(controlName))
                        //{

                        //}
                        //if (StampElement.ContainField(controlName))
                        //{
                        //    _stampFields.Add(StampElement.GetNameInCorrectCase(controlName), 
                        //                     new ElementMicrostation(element));
                        //}                       
                    } 
                }
            }
        }
    }
}
