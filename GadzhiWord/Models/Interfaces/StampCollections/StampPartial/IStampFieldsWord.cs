using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Interfaces.StampCollections.StampPartial
{
    /// <summary>
    /// Поля штампа Word
    /// </summary>
    public interface IStampFieldsWord
    {
        /// <summary>
        /// Получить поля штампа согласно типу
        /// </summary>
        public IEnumerable<IStampTextFieldWord> GetFieldsByType(StampFieldType stampFieldType);

        /// <summary>
        /// Получить строку, начиная от индекса маркера
        /// </summary>
        public IRowElementWord GetTableRowByIndex(int rowIndex, int columnStartIndex, int indexColumnFirst, int fieldsCount);
    }
}