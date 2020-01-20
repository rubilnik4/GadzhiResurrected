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
            await AddFromFilesOrDirectories(filePaths);
        }
       
        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            var directoryPaths = await DialogServiceStandard.OpenFolderDialog(true);
            await AddFromFilesOrDirectories(directoryPaths);
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public async Task AddFromFilesOrDirectories(IEnumerable <string> fileOrDirectoriesPaths)
        {
            ///Поиск файлов на один уровень ниже и в текущей папке
            var filePaths = fileOrDirectoriesPaths?.Where(f => File.Exists(f));
            var directoriesPath = fileOrDirectoriesPaths?.Where(d => Directory.Exists(d));
            var filesInDirectories = directoriesPath?.Union(directoriesPath?.SelectMany(d => Directory.GetDirectories(d)))?
                                                     .SelectMany(d => Directory.GetFiles(d))?
                                                     .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f));
            var allFilePaths = filePaths?.Union(filesInDirectories);
            await Task.FromResult(allFilePaths);

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
