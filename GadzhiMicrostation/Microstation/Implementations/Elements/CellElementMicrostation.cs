using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GadzhiMicrostation.Models.Enums;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public class CellElementMicrostation : RangeBaseElementMicrostation<ICellElementMicrostation>, ICellElementMicrostation
    {
        /// <summary>
        /// Экземпляр ячейки Microstation
        /// </summary>
        private readonly CellElement _cellElement;

        public CellElementMicrostation(CellElement cellElement, IOwnerMicrostation ownerContainerMicrostation)
           : this(cellElement, ownerContainerMicrostation, false)
        { }

       public CellElementMicrostation(CellElement cellElement, IOwnerMicrostation ownerContainerMicrostation,
                                       bool isNeedCompress)
            : base((Element)cellElement, ownerContainerMicrostation, isNeedCompress)
        {
            _cellElement = cellElement ?? throw new ArgumentNullException(nameof(cellElement));
        }

        /// <summary>
        /// Имя ячейки
        /// </summary>
        private string _name;

        /// <summary>
        /// Имя ячейки
        /// </summary>
        public string Name => _name ??= _cellElement.Name;

        /// <summary>
        /// Имя ячейки
        /// </summary>
        private string _description;

        /// <summary>
        /// Имя ячейки
        /// </summary>
        public string Description => _description ??= _cellElement.Description;

        /// <summary>
        /// Масштаб штампа
        /// </summary>
        private double? _scale;

        /// <summary>
        /// Масштаб штампа
        /// </summary>
        private double Scale => _scale ??= _cellElement.Scale.X;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно коэффициента сжатия штампа
        /// </summary>
        public override double UnitScale => Scale * OwnerContainerMicrostation.UnitScale;

        /// <summary>
        /// Координаты базовой точки
        /// </summary>
        private PointMicrostation? _origin;

        /// <summary>
        /// Координаты базовой точки
        /// </summary>
        public override PointMicrostation Origin => _origin ??= _cellElement.Origin.ToPointMicrostation();

        /// <summary>
        /// Дочерние элементы с оригиналами Microstation
        /// </summary>
        private IDictionary<IElementMicrostation, Element> _subElementsPair;

        /// <summary>
        /// Дочерние элементы с оригиналами Microstation
        /// </summary>
        private IDictionary<IElementMicrostation, Element> SubElementsPair => _subElementsPair ??= GetSubElementsPair();

        /// <summary>
        /// Дочерние элементы
        /// </summary>
        public IEnumerable<IElementMicrostation> SubElements => SubElementsPair.Keys;

        /// <summary>
        /// Вписать ячейку в рамку
        /// </summary>
        public override bool CompressRange() => throw new NotImplementedException();

        /// <summary>
        /// Получить дочерние элементы по типу
        /// </summary>
        public IEnumerable<IElementMicrostation> GetSubElementsByType(ElementMicrostationType elementMicrostationType) =>
            SubElements.
            Where(subElement => subElement.ElementType == elementMicrostationType);

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент.Только для внешних операций типа Scale, Move
        /// </summary>
        public void FindAndChangeSubElement(IElementMicrostation elementMicrostation)
        {
            _cellElement.ResetElementEnumeration();
            while (_cellElement.MoveToNextElement())
            {
                var elementCurrent = _cellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == elementMicrostation?.Id)
                {
                    _cellElement.ReplaceCurrentElement(SubElementsPair[elementMicrostation]);
                    elementMicrostation.SetElement((Element)elementCurrent);
                    break;
                }
            }
        }

        /// <summary>
        /// Получить дочерние элементы
        /// </summary>
        private IDictionary<IElementMicrostation, Element> GetSubElementsPair() =>
            _cellElement.GetCellSubElements().
            Where(element => element.IsConvertibleToMicrostation()).
            ToDictionary(element => element.ToElementMicrostation(this),
                         element => element);

        /// <summary>
        /// Переместить элемент
        /// </summary>
        public ICellElementMicrostation Move(PointMicrostation offset) => Move<ICellElementMicrostation>(offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        public ICellElementMicrostation Rotate(PointMicrostation origin, double degree) =>
            Rotate<ICellElementMicrostation>(origin, degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        public ICellElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor) =>
            ScaleAll<ICellElementMicrostation>(origin, scaleFactor);

        /// <summary>
        /// Копировать элемент
        /// </summary>
        public override ICellElementMicrostation Clone() =>
            new CellElementMicrostation(_cellElement, OwnerContainerMicrostation, IsNeedCompress);
    }
}
