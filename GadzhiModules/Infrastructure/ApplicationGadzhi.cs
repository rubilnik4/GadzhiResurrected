using GadzhiModules.Modules.FilesConvertModule.Model;
using GadzhiModules.Helpers;
using GadzhiModules.Infrastructure.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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
        public IDialogServiceStandard DialogServiceStandard { get; }

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public FilesData FilesInfoProject { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 FilesData filesInfoProject)
        {
            DialogServiceStandard = dialogServiceStandard;
            FilesInfoProject = filesInfoProject;
        }
        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            var filePaths = DialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);          
            await Task.Run(() => FilesInfoProject.AddFiles(filePaths));
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public Task AddFromFolders()
        {
            var directoryPaths = DialogServiceStandard.OpenFolderDialog(true); 
            return Task.Run(() =>
            {
                ///Поиск файлов на один уровень ниже
                var filePaths = directoryPaths?.Union(directoryPaths?.SelectMany(d => Directory.GetDirectories(d)))?
                                               .SelectMany(d => Directory.GetFiles(d))?
                                               .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f));
                FilesInfoProject.AddFiles(filePaths);
            });
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
