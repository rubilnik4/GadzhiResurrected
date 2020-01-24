using GadzhiModules.Infrastructure.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System.Windows;

namespace GadzhiModules.Infrastructure.Implementations
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
        /// Проверка состояния папок и файлов
        /// </summary>   
        public IFileSeach FileSeach { get; }

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public IFilesData FilesInfoProject { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSeach fileSeach,
                                 IFilesData filesInfoProject)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSeach = fileSeach;
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
        public async Task AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            //Поиск файлов на один уровень ниже и в текущей папке          
            var filePaths = fileOrDirectoriesPaths?.Where(f => FileSeach.IsFile(f));
            var directoriesPath = fileOrDirectoriesPaths?.Where(d => FileSeach.IsDirectory(d) &&
                                                                     FileSeach.IsDirectoryExist(d));
            var filesInDirectories = directoriesPath?.Union(directoriesPath?.SelectMany(d => FileSeach.GetDirectories(d)))?
                                                     .SelectMany(d => FileSeach.GetFiles(d));
            var allFilePaths = filePaths?.Union(filesInDirectories)?
                                         .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f) &&
                                                     FileSeach.IsFileExist(f));
            await Task.FromResult(allFilePaths);

            if (allFilePaths != null && allFilePaths.Any())
            {
                FilesInfoProject.AddFiles(allFilePaths);
            }          
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            FilesInfoProject.ClearFiles();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesToRemove)
        {
            FilesInfoProject.RemoveFiles(filesToRemove);
        }

        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
