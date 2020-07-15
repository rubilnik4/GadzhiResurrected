using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Services
{
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
        public async Task<PackageDataIntermediateResponseClient> SendFiles(PackageDataRequestClient packageDataRequest) =>
                await _applicationClientConverting.QueueFilesDataAndGetResponse(packageDataRequest);

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid packageId) =>
                await _applicationClientConverting.GetIntermediateFilesDataResponseById(packageId);

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<PackageDataResponseClient> GetCompleteFiles(Guid packageId) =>
                await _applicationClientConverting.GetFilesDataResponseById(packageId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid packageId) =>
              await _applicationClientConverting.SetFilesDataLoadedByClient(packageId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid packageId) => await _applicationClientConverting.AbortConvertingById(packageId);
    }
}
