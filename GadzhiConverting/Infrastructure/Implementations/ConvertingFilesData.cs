using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    public class ConvertingFileData : IConvertingFileData
    {
        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Обработка и конвертирование файла DGN
        /// </summary>
        private readonly IApplicationConverting _applicationConverting;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConvertingFileData(IMessagingService messagingService, IApplicationConverting applicationConverting,
                                  IProjectSettings projectSettings, IFileSystemOperations fileSystemOperations)
        {
            _messagingService = messagingService;
            _applicationConverting = applicationConverting;
            _projectSettings = projectSettings;
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        public async Task<IFileDataServer> Converting(IFileDataServer fileDataServer)
        {
            if (fileDataServer != null)
            {
                fileDataServer = FileDataStartConverting(fileDataServer);

                await FileDataConverting(fileDataServer);

                fileDataServer = FileDataEndConverting(fileDataServer);

                return fileDataServer;
            }
            else
            {
                throw new ArgumentNullException(nameof(fileDataServer));
            }
        }

        /// <summary>
        /// Начать конвертирование файла
        /// </summary>
        private IFileDataServer FileDataStartConverting(IFileDataServer fileDataServer)
        {
            _messagingService.ShowAndLogMessage($"Конвертация файла {fileDataServer.FileNameClient}");

            fileDataServer.StatusProcessing = StatusProcessing.Converting;

            return fileDataServer;
        }

        /// <summary>
        /// Закончить конвертирование файла
        /// </summary>
        private IFileDataServer FileDataEndConverting(IFileDataServer fileDataServer)
        {
            fileDataServer.StatusProcessing = StatusProcessing.ConvertingComplete;
            return fileDataServer;
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>
        private async Task<IFileDataServer> FileDataConverting(IFileDataServer fileDataServer)
        {

            if (fileDataServer.IsValidByAttemptingCount)
            {
                await Task.Run(() => ConvertingFile(fileDataServer, _projectSettings.PrintersInformation));
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.AttemptingCount,
                                                          "Превышено количество попыток конвертирования файла"));
                fileDataServer.AddFileConvertErrorType(FileConvertErrorType.AttemptingCount);
            }

            return fileDataServer;
        }

        /// <summary>
        /// Запустить конвертацию. Инициировать начальные значения
        /// </summary>      
        public IFileDataServer ConvertingFile(IFileDataServer fileDataServer, IPrintersInformation printersInformation)
        {
            _messagingService.ShowAndLogMessage("Загрузка файла");
            _applicationConverting.OpenDocument(fileDataServer?.FilePathServer);

            (IFileDataSourceServer savingSource, ErrorConverting savingErrors) =
                _applicationConverting.SaveDocument(CreateSavingPathByExtension(fileDataServer.FilePathServer,
                                                                                fileDataServer.FileExtentionType));
            _messagingService.ShowAndLogError(savingErrors);


            _messagingService.ShowAndLogMessage("Создание файлов PDF");
            (IEnumerable<IFileDataSourceServer> pdfSource, IEnumerable<ErrorConverting> pdfErrors) = 
                _applicationConverting.CreatePdfFile(CreateSavingPathByExtension(fileDataServer.FilePathServer, FileExtention.pdf),
                                                                                 fileDataServer.ColorPrint, 
                                                                                 printersInformation?.PrintersPdf.FirstOrDefault());
            if (pdfErrors != null)
            {
                foreach (var error in pdfErrors)
                {
                    _messagingService.ShowAndLogError(error);
                }
            }   

            //    _loggerMicrostation.ShowMessage("Создание файла DWG");
            //    _applicationMicrostation.CreateDwgFile(_microstationProject.CreateFileSavePath(_microstationProject.FileDataMicrostation.FileName,
            //                                                                                   FileExtentionMicrostation.dwg));
            _messagingService.ShowAndLogMessage("Конвертирование завершено");
            _applicationConverting.CloseDocument();

            return fileDataServer;
        }

        /// <summary>
        /// Создать папку для сохранения отконвертированных файлов по типу расширения
        /// </summary>       
        private string CreateSavingPathByExtension(string filePathServer, FileExtention fileExtention)
        {
            if (!String.IsNullOrWhiteSpace(filePathServer))
            {
                string serverDirectory = _fileSystemOperations.CreateFolderByName(Path.GetDirectoryName(filePathServer),
                                                                                      fileExtention.ToString());
                return _fileSystemOperations.CombineFilePath(serverDirectory, Path.GetFileNameWithoutExtension(filePathServer),
                                                             fileExtention.ToString());
            }
            else
            {
                throw new ArgumentNullException(nameof(filePathServer));
            }
        }
    }
}
