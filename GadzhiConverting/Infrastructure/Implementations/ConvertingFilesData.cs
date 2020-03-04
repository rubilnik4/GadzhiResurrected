using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations.FilesData;
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
        private readonly IMessagingService _messageAndLoggingService;

        /// <summary>
        /// Обработка и конвертирование файла DGN
        /// </summary>
        private readonly IConvertingFileMicrostation _convertingFileMicrostation;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        public ConvertingFileData(IMessagingService messageAndLoggingService, IConvertingFileMicrostation convertingFileMicrostation, IProjectSettings projectSettings)
        {
            _messageAndLoggingService = messageAndLoggingService;
            _convertingFileMicrostation = convertingFileMicrostation;
            _projectSettings = projectSettings;
        }

        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        public async Task<IFileDataServerConverting> Converting(IFileDataServerConverting fileDataServer)
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
        private IFileDataServerConverting FileDataStartConverting(IFileDataServerConverting fileDataServer)
        {
            _messageAndLoggingService.ShowAndLogMessage($"Конвертация файла {fileDataServer.FileNameClient}");

            fileDataServer.StatusProcessing = StatusProcessing.Converting;

            return fileDataServer;
        }

        /// <summary>
        /// Конвертировать файл
        /// </summary>
        private async Task<IFileDataServerConverting> FileDataConverting(IFileDataServerConverting fileDataServer)
        {

            if (fileDataServer.IsValidByAttemptingCount)
            {
                if (fileDataServer.FileExtentionType == FileExtention.dgn)
                {
                    fileDataServer = await ConvertFileDataMicrostation(fileDataServer);
                }
                else if (fileDataServer.FileExtentionType == FileExtention.docx)
                {
                    await Task.Delay(2000);

                }
            }
            else
            {
                _messageAndLoggingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.AttemptingCount,
                                                          "Превышено количество попыток конвертирования файла"));
                fileDataServer.AddFileConvertErrorType(FileConvertErrorType.AttemptingCount);
            }

            return fileDataServer;
        }

        /// <summary>
        /// Конвертировать файл типа Microstation
        /// </summary>      
        private async Task<IFileDataServerConverting> ConvertFileDataMicrostation(IFileDataServerConverting fileDataServer)
        {
            return await Task.Run(() =>
            {
                FileDataMicrostation convertedFileDataMicrostation = _convertingFileMicrostation.ConvertingFile(
                                                                      ConverterFileDataServerToMicrostation.FileDataServerToMicrostation(fileDataServer),
                                                                      ConverterFileDataServerToMicrostation.PrintersServerToMicrostation(_projectSettings.PrintersInformation));

                return ConverterFileDataServerFromMicrostation.UpdateFileDataServerFromMicrostation(fileDataServer, convertedFileDataMicrostation);
            });

        }

        /// <summary>
        /// Закончить конвертирование файла
        /// </summary>
        private IFileDataServerConverting FileDataEndConverting(IFileDataServerConverting fileDataServer)
        {
            fileDataServer.StatusProcessing = StatusProcessing.ConvertingComplete;
            return fileDataServer;
        }
    }
}
