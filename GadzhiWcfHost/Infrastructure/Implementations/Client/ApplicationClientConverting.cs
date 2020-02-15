using GadzhiDAL.Services.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationClientConverting : IApplicationClientConverting, IAsyncDisposable
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
        private async Task QueueFilesData(FilesDataRequestClient filesDataRequest)
        {
            await _filesDataClientService.QueueFilesData(filesDataRequest);
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<FilesDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid filesDataId)
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse =
                    await _filesDataClientService.GetFilesDataIntermediateResponseById(filesDataId);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<FilesDataResponseClient> GetFilesDataResponseByID(Guid filesDataId)
        {
            FilesDataResponseClient filesDataResponse =
                   await _filesDataClientService.GetFilesDataResponseById(filesDataId);

            return filesDataResponse;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            if (!_authentication.IsClosed)
            {
                await _filesDataClientService?.AbortConvertingById(id);
                _authentication.IsClosed = true;
            }
        }       

        public async Task DisposeAsync()
        {
            await AbortConvertingById(_authentication.Id);            
        }
    }
}