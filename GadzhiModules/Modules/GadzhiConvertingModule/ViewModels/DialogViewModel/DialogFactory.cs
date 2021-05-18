using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Enums.DialogViewModel;
using GadzhiModules.Modules.GadzhiConvertingModule.Views.DialogViews;
using MaterialDesignThemes.Wpf;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel
{
    /// <summary>
    /// Создание диалоговых окон
    /// </summary>
    public static class DialogFactory
    {
        /// <summary>
        /// Создать диалоговое окно информационных сообщений
        /// </summary>
        public static async Task GetMessageDialog(string message)
        {
            var messageDialogViewModel = new MessageDialogViewModel(MessageDialogType.Information, message);
            var messageDialogView = new MessageDialogView(messageDialogViewModel);
            await DialogHost.Show(messageDialogView, "RootDialog");
        }

        /// <summary>
        /// Создать диалоговое окно сообщений с ошибками
        /// </summary>
        public static async Task GetErrorDialog(string message)
        {
            var messageDialogViewModel = new MessageDialogViewModel(MessageDialogType.Error, message);
            var messageDialogView = new MessageDialogView(messageDialogViewModel);
            await DialogHost.Show(messageDialogView, "RootDialog");
        }

        /// <summary>
        /// Создать диалоговое окно с кнопкой подтверждения
        /// </summary>
        public static async Task<bool> GetOkCancelDialog(string message)
        {
            var okCancelDialogViewModel = new OkCancelDialogViewModel(DialogButtonsType.OkCancel, message);
            var okCancelDialogView = new OkCancelDialogView(okCancelDialogViewModel);
            return await DialogHost.Show(okCancelDialogView, "RootDialog").
                   MapAsync(dialogResult => (bool)dialogResult);
        }

        /// <summary>
        /// Создать диалоговое окно с кнопкой подтверждения
        /// </summary>
        public static async Task<bool> GetRetryCancelDialog(string message)
        {
            var okCancelDialogViewModel = new OkCancelDialogViewModel(DialogButtonsType.RetryCancel, message);
            var okCancelDialogView = new OkCancelDialogView(okCancelDialogViewModel);
            return await DialogHost.Show(okCancelDialogView, "RootDialog").
                   MapAsync(dialogResult => (bool)dialogResult);
        }

        /// <summary>
        /// Создать диалоговое окно результатов конвертирования
        /// </summary>
        public static async Task<DialogResultType> GetResultDialog(bool hasErrors)
        {
            var successDialogViewModel = new ResultDialogViewModel(hasErrors);
            var successDialogView = new ResultDialogView(successDialogViewModel);
            return await DialogHost.Show(successDialogView, "RootDialog").
                   MapAsync(dialogResult => (DialogResultType)dialogResult);
        }
    }
}