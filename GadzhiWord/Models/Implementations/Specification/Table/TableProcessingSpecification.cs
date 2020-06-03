using System;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Implementations.Specification.Indexes;
using GadzhiWord.Word.Interfaces.Excel.Elements;

namespace GadzhiWord.Models.Implementations.Specification.Table
{
    /// <summary>
    /// Обработка таблицы спецификации
    /// </summary>
    public static class TableProcessingSpecification
    {
        /// <summary>
        /// Установить ширину колонок
        /// </summary>
        public static Unit SetColumnsWidth(ISheetExcel sheetExcel, SpecificationType specificationType) =>
            specificationType switch
            {
                SpecificationType.Ordinal => SetSpecificationWidth(sheetExcel),
                SpecificationType.Delta => SetSpecificationDeltaWidth(sheetExcel),
                _ => throw new ArgumentOutOfRangeException(nameof(specificationType), specificationType, null)
            };

        /// <summary>
        /// Установить ширину колонок спецификации
        /// </summary>
        private static Unit SetSpecificationWidth(ISheetExcel sheetExcel)
        {
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.POSITION, 7.65f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.NAME, 54f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.MARKING, 25f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.CODE, 18f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.SUPPLIER, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.UNIT, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.QUANTITY, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.WEIGHT, 9.67f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.NOTE, 16.11f);

            return Unit.Value;
        }

        /// <summary>
        /// Установить ширину колонок спецификации с дельтой
        /// </summary>
        private static Unit SetSpecificationDeltaWidth(ISheetExcel sheetExcel)
        {
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.POSITION, 7.65f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.NAME, 54f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.MARKING, 25f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.UNIT, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationIndexes.QUANTITY, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.WEIGHT, 9.67f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.CHANGE_FIRST_DELTA, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.CHANGE_FIRST_TOTAL, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.CHANGE_SECOND_DELTA, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.CHANGE_SECOND_TOTAL, 7.56f);
            sheetExcel.ChangeColumnWidth(SpecificationDeltaIndexes.NOTE, 16.11f);

            return Unit.Value;
        }
    }
}