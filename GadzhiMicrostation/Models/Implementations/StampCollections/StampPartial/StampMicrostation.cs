using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public abstract partial class StampMicrostation : Stamp, IStampMicrostation
    {
        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        public ICellElementMicrostation StampCellElement { get; }

        protected StampMicrostation(ICellElementMicrostation stampCellElement)
        {
            StampCellElement = stampCellElement ?? throw new ArgumentNullException(nameof(stampCellElement));
            StampSubControls = FindElementsInStampFields(stampCellElement.SubElements, StampFieldElement.StampControlNames).ToList();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => StampCellElement.Name;

        /// <summary>
        /// Формат
        /// </summary>
        public override string PaperSize =>
            StampCellElement.SubElements?.
            FirstOrDefault(subElement => subElement.IsTextElementMicrostation &&
                                         StampFieldMain.IsFormatField(subElement.AsTextElementMicrostation.Text))?.
            Map(subElement => StampFieldMain.GetPaperSizeFromField(subElement.AsTextElementMicrostation.Text)) 
            ?? String.Empty;

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override OrientationType Orientation => StampCellElement.Range.Width >= StampCellElement.Range.Height ?
                                                       OrientationType.Landscape :
                                                       OrientationType.Portrait;
    }
}
