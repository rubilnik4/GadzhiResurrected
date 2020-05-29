using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.Fields
{
    /// <summary>
    /// Базовая тестовая ячейка штампа Word
    /// </summary>  
    public class StampTextFieldWord: StampFieldWord, IStampTextFieldWord
    {
        public StampTextFieldWord(ICellElementWord cellElementStamp, StampFieldType stampFieldType)
            :base(cellElementStamp, stampFieldType)
        { }

        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public string Text => CellElementStamp.Text;
    }
}