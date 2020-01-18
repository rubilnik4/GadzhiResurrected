using GadzhiModules.Infrastructure.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiModules.Infrastructure
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary> 
        [Dependency]
        public IDialogServiceStandard DialogServiceStandard { get; set; }

        public ApplicationGadzhi()
        {
        }
        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {           
            var filePaths = DialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
            if (filePaths != null)
            {

            }
            await Task.Delay(2000);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public Task AddFromFolders()
        {
            var filePaths = DialogServiceStandard.OpenFolderDialog(true);
            if (filePaths != null)
            {

            }
            return Task.Delay(2000);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public Task ClearFiles()
        {
            return Task.Delay(2000);
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public Task RemoveFiles()
        {
            return Task.Delay(2000);
        }
    }
}
