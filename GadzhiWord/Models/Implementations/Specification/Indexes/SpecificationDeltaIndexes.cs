namespace GadzhiWord.Models.Implementations.Specification.Indexes
{
    /// <summary>
    /// Данные таблицы спецификации с дельтой
    /// </summary>
    public static class SpecificationDeltaIndexes
    {
        /// <summary>
        /// Количество колонок
        /// </summary>
        public const int COLUMNS_COUNT = 11;

        /// <summary>
        /// Индекс колонки позиции
        /// </summary>
        public const int POSITION = 0;

        /// <summary>
        /// Индекс колонки наименование
        /// </summary>
        public const int NAME = 1;

        /// <summary>
        /// Индекс колонки марка, обозначение
        /// </summary>
        public const int MARKING = 2;

        /// <summary>
        /// Индекс колонки единицы измерения
        /// </summary>
        public const int UNIT = 3;

        /// <summary>
        /// Индекс колонки количество
        /// </summary>
        public const int QUANTITY = 4;

        /// <summary>
        /// Индекс колонки масса
        /// </summary>
        public const int WEIGHT = 5;

        /// <summary>
        /// Изменение 1 дельта
        /// </summary>
        public const int CHANGE_FIRST_DELTA = 6;

        /// <summary>
        /// Изменение 1 всего
        /// </summary>
        public const int CHANGE_FIRST_TOTAL = 7;

        /// <summary>
        /// Изменение 2 дельта
        /// </summary>
        public const int CHANGE_SECOND_DELTA = 8;

        /// <summary>
        /// Изменение 2 всего
        /// </summary>
        public const int CHANGE_SECOND_TOTAL = 9;

        /// <summary>
        /// Индекс колонки примечание
        /// </summary>
        public const int NOTE = 10;
    }
}