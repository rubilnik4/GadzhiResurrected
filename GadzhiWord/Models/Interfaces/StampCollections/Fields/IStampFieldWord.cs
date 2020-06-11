using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Interfaces.StampCollections.Fields
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>  
    public interface IStampFieldWord : IStampField
    {
        /// <summary>
        /// Элемент ячейка, определяющая поле штампа
        /// </summary>
        ICellElementWord CellElementStamp { get; }
    }
}
