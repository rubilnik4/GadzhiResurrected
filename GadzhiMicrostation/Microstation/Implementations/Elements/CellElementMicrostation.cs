using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public class CellElementMicrostation : ElementMicrostation, ICellElementMicrostation
    {
        /// <summary>
        /// Экземпляр ячейки Microstation определяющей штамп
        /// </summary>
        protected readonly CellElement _cellElement;

        public CellElementMicrostation(CellElement cellElement,
                                       IOwnerContainerMicrostation ownerContainerMicrostation)
            : base((Element)cellElement, ownerContainerMicrostation)
        {
            _cellElement = cellElement;
        }

        /// <summary>
        /// Масштаб штампа
        /// </summary>
        private double Scale => _cellElement.Scale.X;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно коэффициента сжатия штампа
        /// </summary>
        public override double UnitScale => Scale * _ownerContainerMicrostation.UnitScale;

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        protected IEnumerable<IElementMicrostation> GetSubElements()
        {
            if (_cellElement != null)
            {
                ElementEnumerator elementEnumerator = _cellElement.GetSubElements();

                while (elementEnumerator.MoveNext())
                {
                    var element = (Element)elementEnumerator.Current;
                    if (element.IsTextElement)
                    {
                        yield return new TextElementMicrostation((TextElement)element, this);
                    }
                    else if (element.IsTextNodeElement)
                    {
                        yield return new TextNodeElementMicrostation((TextNodeElement)element, this);
                    }
                    else if (element.IsCellElement)
                    {
                        yield return new CellElementMicrostation((CellElement)element, this);
                    }
                }
            }
        }

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент. Только для внешних операций типа Scale, Move
        /// </summary>
        protected void FindAndChangeSubElement(long Id)
        {
            while (_cellElement.MoveToNextElement(true))
            {
                var elementCurrent = _cellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == Id)
                {
                    _cellElement.ReplaceCurrentElement(elementCurrent);
                    break;
                }
            }
        }
    }
}
