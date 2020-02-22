using GadzhiDAL.Services.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Implementations.Server
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationServerConverting : IApplicationServerConverting
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>
        private readonly IFilesDataServerService _filesDataServerService;

        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        private Guid _idPackage;

        public ApplicationServerConverting(IFilesDataServerService filesDataServerService)
        {
            _filesDataServerService = filesDataServerService;
            _idPackage = Guid.Empty;
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>           
        public async Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            FilesDataRequestServer filesDataRequestServer = await _filesDataServerService.GetFirstInQueuePackage(identityServerName);
            _idPackage = filesDataRequestServer.Id;
            return filesDataRequestServer;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse)
        {
            await _filesDataServerService.UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task UpdateFromResponse(FilesDataResponseServer filesDataResponse)
        {
            await _filesDataServerService.UpdateFromResponse(filesDataResponse);
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id)
        {
            if (id != Guid.Empty)
            {
                await _filesDataServerService.AbortConvertingById(id);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AbortConvertingById(_idPackage).ConfigureAwait(false);
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