using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public class CellElementMicrostation : RangeBaseElementMicrostation, ICellElementMicrostation
    {
        /// <summary>
        /// Экземпляр ячейки Microstation определяющей штамп
        /// </summary>
        protected CellElement CellElement { get; private set; }

        public CellElementMicrostation(CellElement cellElement,
                                       IOwnerContainerMicrostation ownerContainerMicrostation)
            : base((Element)cellElement, ownerContainerMicrostation, false, false)
        {
            CellElement = cellElement;
        }

        /// <summary>
        /// Масштаб штампа
        /// </summary>
        private double Scale => CellElement.Scale.X;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно коэффициента сжатия штампа
        /// </summary>
        public override double UnitScale => Scale * OwnerContainerMicrostation.UnitScale;

        /// <summary>
        /// Координаты базовой точки
        /// </summary>
        public override PointMicrostation Origin => CellElement.Origin.ToPointMicrostation();

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        protected IEnumerable<IElementMicrostation> GetSubElements()
        {
            if (CellElement != null)
            {
                ElementEnumerator elementEnumerator = CellElement.GetSubElements();

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
            while (CellElement.MoveToNextElement(true))
            {
                var elementCurrent = CellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == Id)
                {
                    CellElement.ReplaceCurrentElement(elementCurrent);
                    break;
                }
            }
        }

        /// <summary>
        /// Вписать ячейку в рамку
        /// </summary>
        public override bool CompressRange()
        {
            throw new NotImplementedException();
        }
    }
}
