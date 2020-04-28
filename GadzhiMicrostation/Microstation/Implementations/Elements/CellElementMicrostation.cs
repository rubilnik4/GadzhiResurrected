﻿using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Extentions.Microstation;
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
    public class CellElementMicrostation : RangeBaseElementMicrostation<ICellElementMicrostation>, ICellElementMicrostation
    {
        /// <summary>
        /// Экземпляр ячейки Microstation определяющей штамп
        /// </summary>
        protected CellElement CellElement { get; private set; }

        public CellElementMicrostation(CellElement cellElement,
                                      IOwnerMicrostation ownerContainerMicrostation)
           : this(cellElement, ownerContainerMicrostation, false, false)
        { }

        public CellElementMicrostation(CellElement cellElement,
                                       IOwnerMicrostation ownerContainerMicrostation,
                                       bool isNeedCompress,
                                       bool isVertical)
            : base((Element)cellElement, ownerContainerMicrostation, isNeedCompress, isVertical)
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
        /// Дочерние элементы с оригиналами Microstation
        /// </summary>
        private IDictionary<IElementMicrostation, Element> _subElementsPair;

        /// <summary>
        /// Дочерние элементы с оригиналами Microstation
        /// </summary>
        private IDictionary<IElementMicrostation, Element> SubElementsPair
        {
            get
            {
                if (_subElementsPair == null)
                {
                    _subElementsPair = GetSubElementsPair();
                }
                return _subElementsPair;
            }
        }

        /// <summary>
        /// Дочерние элементы
        /// </summary>
        public IEnumerable<IElementMicrostation> SubElements => SubElementsPair.Keys;

        /// <summary>
        /// Вписать ячейку в рамку
        /// </summary>
        public override bool CompressRange() => throw new NotImplementedException();

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент.Только для внешних операций типа Scale, Move
        /// </summary>
        public void FindAndChangeSubElement(IElementMicrostation elementMicrostation)
        {
            CellElement.ResetElementEnumeration();
            while (CellElement.MoveToNextElement(true))
            {
                var elementCurrent = CellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == elementMicrostation?.Id)
                {
                    CellElement.ReplaceCurrentElement(SubElementsPair[elementMicrostation]);
                    break;
                }
            }
        }

        /// <summary>
        /// Получить дочерние элементы
        /// </summary>
        private IDictionary<IElementMicrostation, Element> GetSubElementsPair() =>
            CellElement.GetCellSubElements().
            Where(element => element.IsConvertableToMicrostation()).
            ToDictionary(element => element.ToElementMicrostation(this),
                         element => element);

        /// <summary>
        /// Копировать элемент
        /// </summary>     
        public override ICellElementMicrostation Copy(bool isVertical) =>
            new CellElementMicrostation(CellElement, OwnerContainerMicrostation, IsNeedCompress, isVertical);
    }
}
