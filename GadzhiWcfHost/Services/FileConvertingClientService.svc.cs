using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Services
{
    //Обязательно сверять путь с файлом App.config
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется клиентской частью
    /// </summary>   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class FileConvertingClientService : IFileConvertingClientService
    {
        /// <summary>
        /// Сохранение, обработка, подготовка для отправки файлов
        /// </summary>   
        private readonly IApplicationClientConverting _applicationClientConverting;

        public FileConvertingClientService(IApplicationClientConverting applicationClientConverting)
        {
            _applicationClientConverting = applicationClientConverting;
        }

        /// <summary>
        /// Сохранить файлы для конвертации в системе и отправить отчет
        /// </summary>      
        public async Task<FilesDataIntermediateResponseClient> SendFiles(FilesDataRequestClient filesDataRequest)
        {
            var filesDataIntermediateResponse = await _applicationClientConverting.QueueFilesDataAndGetResponse(filesDataRequest);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<FilesDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid id)
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse = await _applicationClientConverting.GetIntermediateFilesDataResponseById(id);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<FilesDataResponseClient> GetCompleteFiles(Guid id)
        {
            FilesDataResponseClient filesDataResponse = await _applicationClientConverting.GetFilesDataResponseByID(id);

            return filesDataResponse;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            await _applicationClientConverting.AbortConvertingById(id);
        }
    }
}
