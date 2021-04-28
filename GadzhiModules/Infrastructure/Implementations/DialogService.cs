using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Infrastructure.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        ///  Выбор файлов
        /// </summary>    
        public IReadOnlyCollection<string> OpenFileDialog(bool isMultiSelect = false, string filter = "")
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = isMultiSelect,
                Filter = filter,
            };

            return openFileDialog.ShowDialog() == true
                ? openFileDialog.FileNames.ToList() 
                : new List<string>();
        }

        /// <summary>
        /// Выбор папки
        /// </summary>     
        public IReadOnlyCollection<string> OpenFolderDialog(bool isMultiSelect = false)
        {
            using var commonOpenFileDialog = new CommonOpenFileDialog { IsFolderPicker = true, Multiselect = isMultiSelect };
            return commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok 
                ? commonOpenFileDialog.FileNames.ToList()
                : new List<string>();
        }

        /// <summary>
        /// Информационное сообщение
        /// </summary>  
        public async Task ShowMessage(string message) => 
            await DialogFactory.GetMessageDialog(message);

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public async Task ShowError(IErrorCommon fileError) =>
            await $"Тип ошибки: {ConverterErrorType.ErrorTypeToString(fileError.ErrorConvertingType)}.\nОписание: {fileError.Description}".
            VoidBindAsync(DialogFactory.GetErrorDialog);

        /// <summary>
        /// Сообщения об ошибках
        /// </summary>
        public async Task ShowErrors(IEnumerable<IErrorCommon> fileErrors)
        {
            if (fileErrors == null) return;

            var fileErrorsCollection = fileErrors.ToList();
            switch (fileErrorsCollection.Count)
            {
                case 0:
                    return;
                case 1:
                    await ShowError(fileErrorsCollection[0]);
                    return;
                default:
                    await DialogFactory.GetErrorDialog("Ошибки записаны в системный журнал");
                    return;
            }
        }

        /// <summary>
        /// Диалоговое окно с подтверждением
        /// </summary>  
        public async Task<bool> ShowMessageOkCancel(string message) => await DialogFactory.GetOkCancelDialog(message);

        /// <summary>
        /// Диалоговое окно с повтором и пропуском
        /// </summary>  
        public async Task<bool> ShowMessageRetryCancel(string message) => await DialogFactory.GetRetryCancelDialog(message);

        /// <summary>
        /// Обертка для функции с диалоговым окном повтора
        /// </summary>
        public async Task RetryOrIgnoreBoolFunction(Func<Task<bool>> asyncFunc, string message)
        {
            bool retry;
            do
            {
                retry = false;
                bool success = await asyncFunc();
                if (!success)
                {
                    retry = await ShowMessageRetryCancel(message);
                }
            } while (retry);
        }
    }
}
