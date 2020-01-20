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
            var filePaths = await DialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
            FilesInfoProject.AddFiles(filePaths);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            var directoryPaths = await DialogServiceStandard.OpenFolderDialog(true);

            ///Поиск файлов на один уровень ниже
            var filePaths = await Task.FromResult(
                 directoryPaths?.Union(directoryPaths?.SelectMany(d => Directory.GetDirectories(d)))?
                                               .SelectMany(d => Directory.GetFiles(d))?
                                               .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f))
            );           

            FilesInfoProject.AddFiles(filePaths);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            // return Task.Delay(2000);
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles()
        {
            //  return Task.Delay(2000);
        }
    }
}
