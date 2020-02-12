using GadzhiDAL.Services.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
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
    public class ApplicationUploadAndGetConverting : IApplicationUploadAndGetConverting
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в клиентской части
        /// </summary>
        private readonly IFilesDataServiceClient _filesDataServiceClient;

        public ApplicationUploadAndGetConverting(IFilesDataServiceClient filesDataService)
        {           
            _filesDataServiceClient = filesDataService;          
        }       

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
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
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        public async Task<FilesDataIntermediateResponse> GetIntermediateFilesDataResponseById(Guid filesDataId)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse =
                    await _filesDataServiceClient.GetIntermediateFilesDataById(filesDataId);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        public async Task<FilesDataResponse> GetFilesDataResponseByID(Guid filesDataServerID)
        {
            //FilesDataServer filesDataServer = _filesDataPackages.GetFilesDataServerByID(filesDataServerID);
            //FilesDataResponse filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
            return null;
        }        
    }
}