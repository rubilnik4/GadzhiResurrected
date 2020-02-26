using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    public class ConvertingFileData : IConvertingFileData
    {
        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        /// <summary>
        /// Обработка и конвертирование файла DGN
        /// </summary>
        private readonly IConvertingFileMicrostation _convertingFileMicrostation;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        public ConvertingFileData(IMessageAndLoggingService messageAndLoggingService, IConvertingFileMicrostation convertingFileMicrostation, IProjectSettings projectSettings)
        {
            _messageAndLoggingService = messageAndLoggingService;
            _convertingFileMicrostation = convertingFileMicrostation;
            _projectSettings = projectSettings;
        }

        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        public async Task<FileDataServer> Converting(FileDataServer fileDataServer)
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
        private FileDataServer FileDataStartConverting(FileDataServer fileDataServer)
        {
            _messageAndLoggingService.ShowMessage($"Конвертация файла {fileDataServer.FileNameWithExtensionClient}");

            fileDataServer.StatusProcessing = StatusProcessing.Converting;

            return fileDataServer;
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>
        private async Task<FileDataServer> FileDataConverting(FileDataServer fileDataServer)
        {

            if (fileDataServer.IsValidByAttemptingCount)
            {
                if (fileDataServer.FileExtensionType == FileExtensions.dgn)
                {
                    FileDataMicrostation convertedFileDataMicrostation = _convertingFileMicrostation.ConvertingFile(
                                                                                    ConverterFileDataServerToMicrostation.FileDataServerToMicrostation(fileDataServer),
                                                                                    ConverterFileDataServerToMicrostation.PrintersServerToMicrostation(_projectSettings.PrintersInformation));
                }
                else if (fileDataServer.FileExtensionType == FileExtensions.docx)
                {
                    await Task.Delay(2000);

                }
            }
            else
            {
                _messageAndLoggingService.ShowError(FileConvertErrorType.AttemptingCount,
                                                    "Превышено количество попыток конвертирования файла");
                fileDataServer.AddFileConvertErrorType(FileConvertErrorType.AttemptingCount);
            }

            return fileDataServer;
        }

        /// <summary>
        /// Закончить конвертирование файла
        /// </summary>
        private FileDataServer FileDataEndConverting(FileDataServer fileDataServer)
        {
            fileDataServer.IsCompleted = true;
            if (fileDataServer.IsValid)
            {
                fileDataServer.StatusProcessing = StatusProcessing.Completed;
            }
            else
            {
                fileDataServer.StatusProcessing = StatusProcessing.Error;
            }

            return fileDataServer;
        }
    }
}
