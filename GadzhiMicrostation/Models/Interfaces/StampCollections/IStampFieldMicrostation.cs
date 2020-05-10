using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа Microstation
    /// </summary>  
    public interface IStampFieldMicrostation: IStampField
    {
        /// <summary>
        /// Текстовый элемент, определяющий поле штампа
        /// </summary>
        IElementMicrostation ElementStamp { get; }
    }
}
