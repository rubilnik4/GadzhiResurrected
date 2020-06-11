using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public interface IStampMicrostation: IStamp, IStampFieldsMicrostation
    {
        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        ICellElementMicrostation StampCellElement { get; }
    }
}
