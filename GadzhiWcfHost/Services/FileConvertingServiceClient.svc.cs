using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Services
{
    //Обязательно сверять путь с файлом App.config
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется и клиентской и серверной частью
    /// </summary>   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class FileConvertingServiceClient : IFileConvertingServiceClient
    {
        /// <summary>
        /// Сохранение, обработка, подготовка для отправки файлов
        /// </summary>   
        private readonly IApplicationUploadAndGetConverting _applicationConverting;

        public FileConvertingServiceClient(IApplicationUploadAndGetConverting applicationReceiveAndSend)
        {
            _applicationConverting = applicationReceiveAndSend;
        }

        /// <summary>
        /// Сохранить файлы для конвертации в системе и отправить отчет
        /// </summary>      
        public async Task<FilesDataIntermediateResponseClient> SendFiles(FilesDataRequestClient filesDataRequest)
        {
            var filesDataIntermediateResponse = await _applicationConverting.QueueFilesDataAndGetResponse(filesDataRequest);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<FilesDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid id)
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse = await _applicationConverting.GetIntermediateFilesDataResponseById(id);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<FilesDataResponseClient> GetCompleteFiles(Guid id)
        {
            FilesDataResponseClient filesDataResponse = await _applicationConverting.GetFilesDataResponseByID(id);

            return filesDataResponse;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            await _applicationConverting.AbortConvertingById(id);
        }
    }
}
