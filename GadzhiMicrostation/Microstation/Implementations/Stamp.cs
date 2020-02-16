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

        public Stamp(CellElement stampCellElement)
        {
            _stampCellElement = stampCellElement;

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
                    if (element.IsTextNodeElement || element.IsTextElement)
                    {
                        string controlName = AttributesElementsMicrostation.
                                             GetAttributeById(element, ElementAttributes.ControlName);

                        if (controlName.ToUpper().Contains ("G_E_OBJNAME_1"))
                        {

                        }
                        //var textNodeElement = (TextNodeElement)element;
                        //switch (textNodeElement.na)

                    }
                }
            }
        }
    }
}
