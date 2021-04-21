using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;
using GadzhiWcfHost.Infrastructure.Implementations.Authentication;

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
        public FileConvertingClientService(IFilesDataClientService filesDataClientService)
        {
            _filesDataClientService = filesDataClientService;
        }

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в клиентской части
        /// </summary>
        private readonly IFilesDataClientService _filesDataClientService;

        /// <summary>
        /// Сохранить файлы для конвертации в системе и отправить отчет
        /// </summary>
        public async Task<PackageDataShortResponseClient> SendFiles(PackageDataRequestClient packageDataRequest)
        {
            if (packageDataRequest == null) return new PackageDataShortResponseClient();
            await QueueFilesData(packageDataRequest, Authentication.GetIdentityName());
            return await CheckFilesStatusProcessing(packageDataRequest.Id);
        }

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<PackageDataShortResponseClient> CheckFilesStatusProcessing(Guid packageId) =>
            await _filesDataClientService.GetFilesDataIntermediateResponseById(packageId);

        /// <summary>
        /// Получить отконвертированный файл по Id номеру
        /// </summary>      
        public async Task<FileDataResponseClient> GetCompleteFile(Guid packageId, string filePath) =>
            await _filesDataClientService.GetFileDataResponseById(packageId, filePath);

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<PackageDataResponseClient> GetCompleteFiles(Guid packageId) =>
            await _filesDataClientService.GetFilesDataResponseById(packageId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid packageId) =>
            await _filesDataClientService.SetFilesDataLoadedByClient(packageId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid packageId) =>
            await _filesDataClientService.AbortConvertingById(packageId);

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName) =>
            await _filesDataClientService.QueueFilesData(packageDataRequest, identityName);
    }
}
