namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Отображение для диалогового окна результатов конвертирования
    /// </summary>
    public class SuccessDialogViewModel
    {
        public SuccessDialogViewModel( bool hasErrors)
        {
            HasErrors = !hasErrors;
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