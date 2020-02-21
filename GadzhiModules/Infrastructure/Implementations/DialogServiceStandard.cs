using GadzhiCommon.Converters;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public class DialogServiceStandard : IDialogServiceStandard
    {
        /// <summary>
        ///  Выбор файлов
        /// </summary>    
        public async Task<IEnumerable<string>> OpenFileDialog(bool isMultiselect = false, string filter = "")
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = isMultiselect,
                Filter = filter,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return await Task.FromResult(openFileDialog.FileNames.ToList());
            }
            return await Task.FromResult(new List<string>());
        }

        /// <summary>
        /// Выбор папки
        /// </summary>     
        public async Task<IEnumerable<string>> OpenFolderDialog(bool isMultiselect = false)
        {
            var commonOpenFileDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = isMultiselect,
            };

            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return await Task.FromResult(commonOpenFileDialog.FileNames);
            }
            return await Task.FromResult(new List<string>());
        }

        /// <summary>
        /// Информационное сообщение
        /// </summary>  
        public void ShowMessage(string messageText)
        {
            MessageBox.Show(messageText);
        }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>  
        public void ShowError(FileConvertErrorType fileConvertErrorType, string fileConvertErrorDescription)
        {
            string messageText = "Ошибка" + "\n" +
                                  ConverterErrorTypeToString.ConvertFileConvertErrorTypeToString(fileConvertErrorType) + "\n" +
                                  fileConvertErrorDescription;

            MessageBox.Show(messageText);
        }

        /// <summary>
        /// Диалоговое окно с подтверждением
        /// </summary>  
        public bool ShowMessageOkCancel(string messageText)
        {
            var dialogResult = MessageBox.Show(messageText, "Gadzhi", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            return dialogResult == DialogResult.OK ?
                   true :
                   false;
        }

        /// <summary>
        /// Диалоговое окно с повтором и пропуском
        /// </summary>  
        public bool ShowMessageRetryCancel(string messageText)
        {
            var dialogResul = MessageBox.Show(messageText, "Gadzhi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);

            return dialogResul == DialogResult.Retry ?
                   true :
                   false;
        }

        /// <summary>
        /// Обертка для функции с диалоговым окном повтора
        /// </summary>
        public async Task RetryOrIgnoreBoolFunction(Func<Task<bool>> asyncFunc, string messageText)
        {
            bool retry = false;
            do
            {
                retry = false;
                bool succsess = await asyncFunc();
                if (!succsess)
                {
                    retry = ShowMessageRetryCancel(messageText);
                }
            } while (retry);
        }
    }
}
