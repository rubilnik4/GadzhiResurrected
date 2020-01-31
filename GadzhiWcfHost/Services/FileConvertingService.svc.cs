using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Implementations;
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
    //[DeliveryRequirements(RequireOrderedDelivery = true)]
    public class FileConvertingService : IFileConvertingService, IDisposable
    {
        /// <summary>
        /// Сохранение, обработка, подготовка для отправки файлов
        /// </summary>   
        private IApplicationConverting ApplicationReceiveAndSend { get; }

        public FileConvertingService(IApplicationConverting applicationReceiveAndSend)
        {
            ApplicationReceiveAndSend = applicationReceiveAndSend;          
        }

        /// <summary>
        /// Сохранить файлы для конвертации в системе и отправить отчет
        /// </summary>      
        public async Task<FilesDataIntermediateResponse> SendFiles(FilesDataRequest filesDataRequest)
        {
            var filesDataIntermediateResponse = await ApplicationReceiveAndSend.QueueFilesDataAndGetResponse(filesDataRequest);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Проверить статус файлов
        /// </summary>      
        public async Task<FilesDataIntermediateResponse> CheckFilesStatusProcessing(Guid filesDataID)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse = await ApplicationReceiveAndSend.GetIntermediateResponseByID(filesDataID);

            return filesDataIntermediateResponse;
        }

        public void Dispose()
        {
          
        }
    }
}
