using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOServer.TransferModels.Signatures;
using System.Linq;
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
        public async Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName)=>
                await _applicationServerConverting.GetFirstInQueuePackage(identityServerName);       

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse) =>
                await _applicationServerConverting.UpdateFromIntermediateResponse(packageDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task UpdateFromResponse(PackageDataResponseServer packageDataResponse) =>
                await _applicationServerConverting.UpdateFromResponse(packageDataResponse);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion) =>
                await _applicationServerConverting.DeleteAllUnusedPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        public async Task DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion) =>
                await _applicationServerConverting.DeleteAllUnusedErrorPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id) => await _applicationServerConverting.AbortConvertingById(id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => await _applicationServerConverting.GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) => await _applicationServerConverting.GetSignatures(ids);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto) => await _applicationServerConverting.UploadSignatures(signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetSignaturesMicrostation() => await _applicationServerConverting.GetSignaturesMicrostation();

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetStampsMicrostation() => await _applicationServerConverting.GetStampsMicrostation();

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _applicationServerConverting.UploadSignaturesMicrostation(microstationDataFileDto);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        public async Task UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _applicationServerConverting.UploadStampsMicrostation(microstationDataFileDto);
    }
}
