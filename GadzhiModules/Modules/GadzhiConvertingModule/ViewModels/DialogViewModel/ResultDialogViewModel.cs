namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Диалоговое окно результатов конвертирования
    /// </summary>
    public class ResultDialogViewModel
    {
        public ResultDialogViewModel(bool hasErrors)
        {
            HasErrors = hasErrors;
        }

        /// <summary>
        /// Наличие ошибок
        /// </summary>
        public bool HasErrors { get; }

        /// <summary>
        /// Отсутствие ошибок
        /// </summary>
        public bool NoErrors => !HasErrors;
    }
}