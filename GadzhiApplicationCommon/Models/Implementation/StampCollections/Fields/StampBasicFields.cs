using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Обязательные поля штампа
    /// </summary>
    public class StampBasicFields : IStampBasicFields
    {
        public StampBasicFields(IStampTextField code, IStampTextField currentSheetNumber)
        {
            FullCode = code ?? throw new ArgumentNullException(nameof(code));
            CurrentSheet = currentSheetNumber ?? throw new ArgumentNullException(nameof(currentSheetNumber));
        }

        /// <summary>
        /// Шифр
        /// </summary>
        public IStampTextField FullCode { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        public IStampTextField CurrentSheet { get; }

        /// <summary>
        /// Проверить шифр на корректность
        /// </summary>
        public static bool ValidateFullCode(string fullCode) => !fullCode.IsNullOrWhiteSpace();

        /// <summary>
        /// Проверить текущий лист на корректность
        /// </summary>
        public static bool ValidateCurrentSheet(string currentSheet) => Int32.TryParse(currentSheet, out int _);

        /// <summary>
        /// Перевести номер листа в строковый формат
        /// </summary>
        public static string CurrentSheetFormat(int currentSheet) => (currentSheet < 10)
                                                                        ? $"{ currentSheet:00}"
                                                                        : $"{ currentSheet}";
    }
}