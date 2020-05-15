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
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id) => await _applicationServerConverting.AbortConvertingById(id);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto) =>
            await _applicationServerConverting.UploadSignatures(signaturesDto);

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task UploadSignaturesMicrostation(SignatureMicrostationDto signaturesMicrostationDto) =>
            await _applicationServerConverting.UploadSignaturesMicrostation(signaturesMicrostationDto);
    }
}
