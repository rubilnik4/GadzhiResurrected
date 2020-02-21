using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.FilesConvert.Implementations;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    public class ConvertingFileData : IConvertingFileData
    {
        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        public ConvertingFileData(IMessageAndLoggingService messageAndLoggingService)
        {
            _messageAndLoggingService = messageAndLoggingService;
        }

        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        public async Task<FileDataServer> Converting(FileDataServer fileDataServer)
        {
            fileDataServer = FileDataStartConverting(fileDataServer);

            await FileDataConverting(fileDataServer);

            fileDataServer = FileDataEndConverting(fileDataServer);

            return fileDataServer;
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
            await Task.Delay(2000);

            if (fileDataServer.IsValidByAttemptingCount)
            {

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
