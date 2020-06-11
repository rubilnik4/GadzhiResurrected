using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public interface IStampMicrostation: IStamp
    {
        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        ICellElementMicrostation StampCellElement { get; }
    }
}
