using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Services
{
    //Обязательно сверять путь с файлом App.config
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется серверной частью
    /// </summary>   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class FileConvertingServerService : IFileConvertingServerService
    {
        /// <summary>
        /// Класс для отправки пакетов на сервер
        /// </summary>
        private readonly IApplicationServerConverting _applicationServerConverting;

        public FileConvertingServerService(IApplicationServerConverting applicationServerConverting)
        {
            _applicationServerConverting = applicationServerConverting;
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>           
        public async Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            return await _applicationServerConverting.GetFirstInQueuePackage(identityServerName);
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse)
        {
            await _applicationServerConverting.UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task UpdateFromResponse(FilesDataResponseServer filesDataResponse)
        {
            await _applicationServerConverting.UpdateFromResponse(filesDataResponse);
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id)
        {
            await _applicationServerConverting.AbortConvertingById(id);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationServerConverting.Dispose();
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
