using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public partial class Stamp : CellElementMicrostation, IStamp
    {
        public Stamp(CellElement stampCellElement,
                     IOwnerContainerMicrostation ownerModelMicrostation)
            : base(stampCellElement, ownerModelMicrostation)
        {
            InitialStampDataFields();
        }

        /// <summary>
        /// Тип расположения штапа
        /// </summary>
        public OrientationType Orientation => Range.Width >= Range.Height ?
                                              OrientationType.Horizontal :
                                              OrientationType.Vertical;
    }
}
