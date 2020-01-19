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
        /// Класс содержащий данные о конвертируемых файлах
        /// </summary>       
        public FilesInfo FilesInfoProject { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 FilesInfo filesInfoProject)
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
            FilesInfoProject.AddFiles(filePaths);

            await Task.Delay(2000);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public Task AddFromFolders()
        {
            var directoryPaths = DialogServiceStandard.OpenFolderDialog(true);

            ///Поиск файлов на один уровень ниже
            var filePaths = directoryPaths?.Union(directoryPaths?.SelectMany(d => Directory.GetDirectories(d)))?
                                           .SelectMany(d => Directory.GetFiles(d))?
                                           .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f));
            FilesInfoProject.AddFiles(filePaths);

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
