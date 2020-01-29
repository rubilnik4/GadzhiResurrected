using GadzhiModules.Infrastructure.Interfaces;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            OpenFileDialog openFileDialog = new OpenFileDialog()
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
        /// Диалоговое окно с подтверждением
        /// </summary>  
        public bool ShowMessageOkCancel(string messageText)
        {
            var dialogResul = MessageBox.Show(messageText, "Gadzhi", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            return dialogResul == MessageBoxResult.OK ? 
                   true : 
                   false;
        }
    }
}
