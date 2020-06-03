using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Implementations.Specification.Indexes;
using GadzhiWord.Models.Interfaces.Specification;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.Specification
{
    /// <summary>
    /// Спецификация
    /// </summary>
    public class Specification : ISpecification
    {
        public Specification(IEnumerable<ITableElementWord> tablesWord)
        {
            var tablesWordCollection = tablesWord?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(tablesWord));
            if (!IsTablesSpecification(tablesWordCollection)) throw new ArgumentException("Таблицы не являются спецификациями одного типа");

            TablesWord = tablesWordCollection;
        }

        /// <summary>
        /// Лист приложения Excel
        /// </summary>
        public IReadOnlyCollection<ITableElementWord> TablesWord { get; }

        /// <summary>
        /// Определить тип спецификации
        /// </summary>
        public SpecificationType GetSpecificationType() =>
            TablesWord.FirstOrDefault()?.ColumnsCountInitial switch
            {
                SpecificationIndexes.COLUMNS_COUNT => SpecificationType.Ordinal,
                SpecificationDeltaIndexes.COLUMNS_COUNT => SpecificationType.Delta,
                _ => throw new InvalidEnumArgumentException(nameof(TablesWord), 0, typeof(int)),
            };

        /// <summary>
        /// Являются ли таблицы Word спецификациями
        /// </summary>
        public static bool IsTablesSpecification(IList<ITableElementWord> tablesWord) =>
            tablesWord.Count > 0 &&
            (tablesWord.All(IsTableSpecificationOrdinal) || tablesWord.All(IsTableSpecificationDelta));

        /// <summary>
        /// Является ли таблица Word обычной спецификацией
        /// </summary>
        public static bool IsTableSpecification(ITableElementWord tableWord) =>
            IsTableSpecificationOrdinal(tableWord) || IsTableSpecificationDelta(tableWord);

        /// <summary>
        /// Является ли таблица Word обычной спецификацией
        /// </summary>
        public static bool IsTableSpecificationOrdinal(ITableElementWord tableWord) =>
            tableWord.ColumnsCountInitial == SpecificationIndexes.COLUMNS_COUNT;

        /// <summary>
        /// Является ли таблица Word спецификацией с дельтой
        /// </summary>
        public static bool IsTableSpecificationDelta(ITableElementWord tableWord) =>
            tableWord.ColumnsCountInitial == SpecificationDeltaIndexes.COLUMNS_COUNT;
    }
}