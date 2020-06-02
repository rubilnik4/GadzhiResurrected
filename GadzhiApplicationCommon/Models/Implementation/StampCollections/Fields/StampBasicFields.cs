using System;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Обязательные поля штампа
    /// </summary>
    public class StampBasicFields : IStampBasicFields
    {
        public StampBasicFields(IResultAppValue<IStampTextField> fullCode, IResultAppValue<IStampTextField> currentSheet)
        {
            if (fullCode == null) throw new ArgumentNullException(nameof(fullCode));
            if (currentSheet == null) throw new ArgumentNullException(nameof(currentSheet));

            FullCode = GetValidFullCode(fullCode);
            CurrentSheet = GetValidCurrentNumber(currentSheet);
        }

        /// <summary>
        /// Шифр
        /// </summary>
        public IResultAppValue<IStampTextField> FullCode { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        public IResultAppValue<IStampTextField> CurrentSheet { get; }

        /// <summary>
        /// Номер текущего листа в числовом формате
        /// </summary>
        public IResultAppValue<int> CurrentSheetNumber => CurrentSheet.ResultValueOk(currentSheet => currentSheet.Text.ParseInt());

        /// <summary>
        /// Проверить шифр на корректность
        /// </summary>
        public static bool ValidateFullCode(string fullCode) => !fullCode.IsNullOrWhiteSpace();

        /// <summary>
        /// Проверить текущий лист на корректность
        /// </summary>
        public static bool ValidateCurrentSheet(string currentSheet) => Int32.TryParse(currentSheet, out int _);

        /// <summary>
        /// Получить поле шифра с учетом проверки на ошибки
        /// </summary>
        private static IResultAppValue<IStampTextField> GetValidFullCode(IResultAppValue<IStampTextField> fullCodeResult) =>
            fullCodeResult.
            ResultValueContinue(fullCode => ValidateFullCode(fullCode.Text),
                okFunc: fullCode => fullCode,
                badFunc: fullCode => new ErrorApplication(ErrorApplicationType.FieldNotFound, "Не задано значение поля шифра"));

        /// <summary>
        /// Получить поле номера листа с учетом проверки на ошибки
        /// </summary>
        private static IResultAppValue<IStampTextField> GetValidCurrentNumber(IResultAppValue<IStampTextField> currentNumberResult) =>
            currentNumberResult.
            ResultValueContinue(currentNumber => ValidateCurrentSheet(currentNumber.Text),
                okFunc: currentNumber => currentNumber,
                badFunc: fullCode => new ErrorApplication(ErrorApplicationType.FieldNotFound, "Значение поля текущего листа не представлено числом"));
    }
}