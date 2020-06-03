using System.Collections.Generic;
using GadzhiWord.Models.Enums;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Interfaces.Specification
{
    /// <summary>
    /// Спецификация
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        /// Лист приложения Excel
        /// </summary>
        public IReadOnlyCollection<ITableElementWord> TablesWord { get; }

        /// <summary>
        /// Определить тип спецификации
        /// </summary>
        SpecificationType GetSpecificationType();
    }
}