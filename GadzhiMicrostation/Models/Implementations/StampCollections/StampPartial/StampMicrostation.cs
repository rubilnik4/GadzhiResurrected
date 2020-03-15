using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract partial class StampMicrostation : Stamp
    {
        private readonly ICellElementMicrostation _stampCellElement;

        public StampMicrostation(ICellElementMicrostation stampCellElement)
        {
            _stampCellElement = stampCellElement;
           // InitializeStampFields();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => _stampCellElement.Name;

        /// <summary>
        /// Формат
        /// </summary>
        public override string PaperSize =>
            _stampCellElement.SubElements?.
            Where(subElement => subElement.IsTextElementMicrostation &&
                                subElement.AttributeControlName == StampFieldMain.PaperSize.Name).
            Select(subElement => StampFieldMain.GetPaperSizeFromField(subElement.AsTextElementMicrostation.Text)).
            FirstOrDefault() ??
            String.Empty;
                                                                 

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override OrientationType Orientation => _stampCellElement.Range.Width >= _stampCellElement.Range.Height ?
                                              OrientationType.Landscape :
                                              OrientationType.Portrait;
    }
}
