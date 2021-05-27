using System.IO;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Infrastructure.Implementations.Converting
{
    /// <summary>
    /// Обработка пути
    /// </summary>
    public static class ConvertingFilePath
    {
        /// <summary>
        /// Получить коллекцию путей
        /// </summary>
        /// <returns></returns>
        public static FilePathCollection GetFilePathCollection(IFileDataServer fileDataServer, IFileSystemOperations fileSystemOperations) =>
             new FilePathCollection(CreateSavingPath(fileDataServer.FilePathServer, fileDataServer.FileExtensionType, fileSystemOperations).
                                    Map(fileDataServer.ChangeServerPath),
                                    CreateSavingPath(fileDataServer.FilePathServer, FileExtensionType.Pdf, fileSystemOperations).
                                    Map(fileDataServer.ChangeServerPath),
                                    CreateSavingPath(fileDataServer.FilePathServer, FileExtensionType.Print, fileSystemOperations).
                                    Map(fileDataServer.ChangeServerPath));

        /// <summary>
        /// Создать папку для сохранения отконвертированных файлов по типу расширения
        /// </summary>       
        public static string CreateSavingPath(string filePathServer, FileExtensionType fileExtensionType, 
                                              IFileSystemOperations fileSystemOperations) =>
            Path.GetDirectoryName(filePathServer).
            Map(directory => fileSystemOperations.CreateFolderByName(Path.Combine(directory, Path.GetFileNameWithoutExtension(filePathServer)),
                                                                     fileExtensionType.ToString())).
            Map(serverDirectory => FilePathOperations.CombineFilePath(serverDirectory,
                                                                      Path.GetFileNameWithoutExtension(filePathServer),
                                                                      fileExtensionType.ToString().ToLowerCaseCurrentCulture()));

    }
}