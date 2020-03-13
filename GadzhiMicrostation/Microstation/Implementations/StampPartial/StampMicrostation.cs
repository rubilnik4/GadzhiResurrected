using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public partial class StampMicrostation : CellElementMicrostation, IStampMicrostation
    {
        public StampMicrostation(CellElement stampCellElement,
                                 IOwnerContainerMicrostation ownerModelMicrostation)
            : base(stampCellElement, ownerModelMicrostation)
        {
            InitialStampDataFields();
        }

        /// <summary>
        /// Тип расположения штапа
        /// </summary>
        public OrientationType Orientation => Range.Width >= Range.Height ?
                                              OrientationType.Landscape :
                                              OrientationType.Portrait;
    }
}
