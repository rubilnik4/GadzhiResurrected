using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using MicroStationDGN;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract partial class Stamp : CellElementMicrostation, IStamp
    {
        public Stamp(CellElement stampCellElement, IOwnerContainerMicrostation ownerModelMicrostation)
             : base(stampCellElement, ownerModelMicrostation)
        {           
            InitializeStampFields();

        }       
        /// <summary>
        /// Тип штампа
        /// </summary>
        public abstract StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize => StampFieldMain.GetPaperSizeFromField(StampFieldsWrapper[StampFieldMain.PaperSize.Name]?.AsTextElementMicrostation?.Text) ??
                                   string.Empty;

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public OrientationType Orientation => Range.Width >= Range.Height ?
                                              OrientationType.Landscape :
                                              OrientationType.Portrait;
    }
}
