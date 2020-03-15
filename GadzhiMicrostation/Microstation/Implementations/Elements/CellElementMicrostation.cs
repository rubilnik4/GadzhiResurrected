using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Converters;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Имя ячейки
        /// </summary>
        public string Name => CellElement.Name;

        /// <summary>
        /// Имя ячейки
        /// </summary>
        public string Description => CellElement.Description;

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
        /// Получить дочерние элементы
        /// </summary>
        public IEnumerable<IElementMicrostation> SubElements => GetSubElementsPair().
                                                                Select(elementPair => elementPair.ElementWrapper);

        /// <summary>
        /// Вписать ячейку в рамку
        /// </summary>
        public override bool CompressRange() => throw new NotImplementedException();     

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        private IEnumerable<ElementMicrostationPair> GetSubElementsPair()
        {
            if (CellElement != null)
            {
                ElementEnumerator elementEnumerator = CellElement.GetSubElements();

                while (elementEnumerator.MoveNext())
                {
                    var microstationElement = ConvertMicrostationElements.ConvertToMicrostationElement((Element)elementEnumerator.Current, this);
                    if (microstationElement != null)
                    {
                        yield return new ElementMicrostationPair(microstationElement, (Element)elementEnumerator.Current);
                    }

                }
            }
        }

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент. Только для внешних операций типа Scale, Move
        /// </summary>
        private void FindAndChangeSubElement(Element element)
        {
            CellElement.ResetElementEnumeration();
            while (CellElement.MoveToNextElement(true))
            {
                var elementCurrent = CellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == element?.ID64)
                {                   
                    CellElement.ReplaceCurrentElement(element);
                    break;
                }
            }
        }        
    }
}
