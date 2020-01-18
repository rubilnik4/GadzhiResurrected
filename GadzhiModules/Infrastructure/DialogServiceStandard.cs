using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure
{
    /// <summary>
    /// Стандартные диалоговые окна
    /// </summary>
    public class DialogServiceStandard : IDialogServiceStandard
    {
        /// <summary>
        ///  Выбор файлов
        /// </summary>    
        public IEnumerable<string> OpenFileDialog(bool isMultiselect = false, string filter = "")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = isMultiselect,
                Filter = filter,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames.ToList();
            }
            return null;
        }

        /// <summary>
        /// Выбор папки
        /// </summary>     
        public IEnumerable<string> OpenFolderDialog(bool isMultiselect = false)
        {
            var commonOpenFileDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = isMultiselect,
            };

            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return commonOpenFileDialog.FileNames;               
            }         
            return null;
        }
    }
}
