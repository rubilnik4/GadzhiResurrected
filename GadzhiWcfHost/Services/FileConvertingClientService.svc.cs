using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using Microsoft.VisualStudio.Threading;
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
        public async Task<PackageDataIntermediateResponseClient> SendFiles(PackageDataRequestClient packageDataRequest) =>
                await _applicationClientConverting.QueueFilesDataAndGetResponse(packageDataRequest);

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid filesDataId) =>
                await _applicationClientConverting.GetIntermediateFilesDataResponseById(filesDataId);

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<PackageDataResponseClient> GetCompleteFiles(Guid filesDataId) =>
                await _applicationClientConverting.GetFilesDataResponseByID(filesDataId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid filesDataId) =>
              await _applicationClientConverting.SetFilesDataLoadedByClient(filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id) => await _applicationClientConverting.AbortConvertingById(id);
       

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationClientConverting.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {            
            Dispose(true);          
        }
        #endregion


    }
}
