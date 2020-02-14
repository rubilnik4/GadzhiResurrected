using GadzhiDAL.Services.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationUploadAndGetConverting : IApplicationUploadAndGetConverting, IAsyncDisposable
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в клиентской части
        /// </summary>
        private readonly IFilesDataServiceClient _filesDataServiceClient;

        /// <summary>
        /// Идентефикация пользователя
        /// </summary>
        private readonly IAuthentication _authentication;

        public ApplicationUploadAndGetConverting(IFilesDataServiceClient filesDataService,
                                                  IAuthentication authentication)
        {
            _filesDataServiceClient = filesDataService;
            _authentication = authentication;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            filesDataRequest = _authentication.AuthenticateFilesData(filesDataRequest);

            await QueueFilesData(filesDataRequest);

            return await GetIntermediateFilesDataResponseById(filesDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(FilesDataRequest filesDataRequest)
        {
            await _filesDataServiceClient.QueueFilesData(filesDataRequest);
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<FilesDataIntermediateResponse> GetIntermediateFilesDataResponseById(Guid filesDataId)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse =
                    await _filesDataServiceClient.GetFilesDataIntermediateResponseById(filesDataId);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<FilesDataResponse> GetFilesDataResponseByID(Guid filesDataId)
        {
            FilesDataResponse filesDataResponse =
                   await _filesDataServiceClient.GetFilesDataResponseById(filesDataId);

            return filesDataResponse;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            if (!_authentication.IsClosed)
            {
                await _filesDataServiceClient?.AbortConvertingById(id);
                _authentication.IsClosed = true;
            }
        }       

        public async Task DisposeAsync()
        {
            await AbortConvertingById(_authentication.Id);            
        }
    }
}