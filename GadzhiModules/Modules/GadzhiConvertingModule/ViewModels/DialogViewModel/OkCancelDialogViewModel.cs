using System;
using System.ComponentModel;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Диалоговое окно с кнопкой подтверждения
    /// </summary>
    public class OkCancelDialogViewModel
    {
        public OkCancelDialogViewModel(DialogButtonsType dialogButtonsType, string message)
        {
            DialogButtonsType = dialogButtonsType;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// Типы кнопок для диалогового окна
        /// </summary>
        public DialogButtonsType DialogButtonsType { get; }

        /// <summary>
        /// Информационное сообщение
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Текст первой кнопки
        /// </summary>
        public string FirstButtonText =>
            DialogButtonsType switch
            {
                DialogButtonsType.OkCancel => "ОК",
                DialogButtonsType.RetryCancel => "RETRY",
                _ => "ОК",
            };

        /// <summary>
        /// Текст второй кнопки
        /// </summary>
        public string SecondButtonText => "ОТМЕНА";
    }
}