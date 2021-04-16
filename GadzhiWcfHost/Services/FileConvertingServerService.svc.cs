using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOServer.TransferModels.Signatures;
using System.Linq;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiWcfHost.Services
{
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется серверной частью
    /// </summary>   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class FileConvertingServerService : IFileConvertingServerService
    {
        public FileConvertingServerService(IFilesDataServerService filesDataServerService)
        {
            _filesDataServerService = filesDataServerService;
        }

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
        /// </summary>
        private readonly IFilesDataServerService _filesDataServerService;

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>           
        public async Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName)=>
                await _filesDataServerService.GetFirstInQueuePackage(identityServerName);       

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(Guid packageId, FileDataResponseServer fileDataResponseServer) =>
                await _filesDataServerService.UpdateFromIntermediateResponse(packageId, fileDataResponseServer);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task<Unit> UpdateFromResponse(PackageDataShortResponseServer packageDataShortResponseServer) =>
                await _filesDataServerService.UpdateFromResponse(packageDataShortResponseServer);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion) =>
                await _filesDataServerService.DeleteAllUnusedPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion) =>
                await _filesDataServerService.DeleteAllUnusedErrorPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task<Unit> AbortConvertingById(Guid id) => 
            await _filesDataServerService.AbortConvertingById(id);
    }
}
