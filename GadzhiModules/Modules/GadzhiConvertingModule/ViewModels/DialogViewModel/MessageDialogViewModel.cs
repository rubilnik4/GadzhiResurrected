using System;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Диалоговое окно информационного сообщения
    /// </summary>
    public class MessageDialogViewModel
    {
        public MessageDialogViewModel(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// Информационное сообщение
        /// </summary>
        public string Message { get; }
    }
}