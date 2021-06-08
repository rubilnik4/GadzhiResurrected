using System;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Enums.DialogViewModel;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Base;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Диалоговое окно информационного сообщения
    /// </summary>
    public class MessageDialogViewModel : DialogViewModelBase
    {
        public MessageDialogViewModel(MessageDialogType messageDialogType, string message)
        {
            MessageDialogType = messageDialogType;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// Тип диалогового окна для сообщений
        /// </summary>
        public MessageDialogType MessageDialogType { get; }

        /// <summary>
        /// Информационное сообщение
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public override string Title =>
            MessageDialogType switch
            {
                MessageDialogType.Information => "Сообщение",
                MessageDialogType.Error => "Ошибка",
                _ => "Сообщение",
            };

        /// <summary>
        /// Является ли окно информационным типом
        /// </summary>
        public bool IsInformationType => MessageDialogType == MessageDialogType.Information;

        /// <summary>
        /// Является ли окно информационным типом
        /// </summary>
        public bool IsErrorType => MessageDialogType == MessageDialogType.Error;

    }
}