using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.Fields
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
