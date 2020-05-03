using GadzhiDAL.Services.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;

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

        public ApplicationClientConverting(IFilesDataClientService filesDataClientService)
        {
            _filesDataClientService = filesDataClientService;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> QueueFilesDataAndGetResponse(PackageDataRequestClient packageDataRequest)
        {
            if (packageDataRequest == null) return new PackageDataIntermediateResponseClient();

            await QueueFilesData(packageDataRequest, Authentication.GetIdentityName());
            return await GetIntermediateFilesDataResponseById(packageDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName) =>      
                await _filesDataClientService.QueueFilesData(packageDataRequest, identityName);
       

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid filesDataServerId) =>      
                await _filesDataClientService.GetFilesDataIntermediateResponseById(filesDataServerId);       

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid filesDataServerId) =>
               await _filesDataClientService.GetFilesDataResponseById(filesDataServerId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid filesDataId) =>
              await _filesDataClientService.SetFilesDataLoadedByClient(filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id) => await _filesDataClientService.AbortConvertingById(id);
    }
}