using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;

namespace GadzhiMicrostation.Microstation.Interfaces.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public interface IStamp : ICellElementMicrostation, ISignaturesStamp, IStampDataFields
    {
        /// <summary>
        /// Тип расположения штапа
        /// </summary>
        OrientationType Orientation { get; }
    }
}
