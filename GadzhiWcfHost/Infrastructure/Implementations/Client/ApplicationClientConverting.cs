using GadzhiDAL.Services.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationClientConverting : IApplicationClientConverting
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в клиентской части
        /// </summary>
        private readonly IFilesDataClientService _filesDataClientService;

        /// <summary>
        /// Идентефикация пользователя
        /// </summary>
        private readonly IAuthentication _authentication;

        public ApplicationClientConverting(IFilesDataClientService filesDataClientService,
                                                  IAuthentication authentication)
        {
            _filesDataClientService = filesDataClientService;
            _authentication = authentication;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponseClient> QueueFilesDataAndGetResponse(FilesDataRequestClient filesDataRequest)
        {
            filesDataRequest = _authentication.AuthenticateFilesData(filesDataRequest);

            await QueueFilesData(filesDataRequest);

            return await GetIntermediateFilesDataResponseById(filesDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(FilesDataRequestClient filesDataRequest) =>      
                await _filesDataClientService.QueueFilesData(filesDataRequest);
       

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<FilesDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid filesDataId) =>      
                await _filesDataClientService.GetFilesDataIntermediateResponseById(filesDataId);       

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<FilesDataResponseClient> GetFilesDataResponseByID(Guid filesDataId) =>
               await _filesDataClientService.GetFilesDataResponseById(filesDataId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid filesDataId) =>
              await _filesDataClientService.SetFilesDataLoadedByClient(filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            if (!_authentication.IsClosed)
            {
                if (_authentication.Id != Guid.Empty)
                {
                    await _filesDataClientService?.AbortConvertingById(id);
                }
                _authentication.IsClosed = true;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AbortConvertingById(_authentication.Id).ConfigureAwait(false);
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