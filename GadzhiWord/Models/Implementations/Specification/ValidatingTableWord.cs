using GadzhiWord.Word.Interfaces.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.Specification
{
    /// <summary>
    /// Проверка на наличие таблицы Word
    /// </summary>
    public static class ValidatingTableWord
    {
        /// <summary>
        /// Является ли таблица Word спецификацией
        /// </summary>
        public static bool IsTableSpecification(ITableElementWord tableElement) =>
            tableElement.ColumnsCountInitial == ColumnIndexesSpecification.COLUMNS_COUNT;
    }
}