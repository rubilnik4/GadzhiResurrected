﻿using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

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
        /// <summary>
        /// Сохранение, обработка, подготовка для отправки файлов
        /// </summary>   
        private readonly IApplicationClientConverting _applicationClientConverting;

        public FileConvertingClientService(IApplicationClientConverting applicationClientConverting)
        {
            _applicationClientConverting = applicationClientConverting;
        }

        /// <summary>
        /// Сохранить файлы для конвертации в системе и отправить отчет
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> SendFiles(PackageDataRequestClient packageDataRequest) =>
                await _applicationClientConverting.QueueFilesDataAndGetResponse(packageDataRequest);

        /// <summary>
        /// Проверить статус файлов по Id номеру
        /// </summary>      
        public async Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid filesDataId) =>
                await _applicationClientConverting.GetIntermediateFilesDataResponseById(filesDataId);

        /// <summary>
        /// Отправить отконвертированные файлы по Id номеру
        /// </summary>      
        public async Task<PackageDataResponseClient> GetCompleteFiles(Guid filesDataId) =>
                await _applicationClientConverting.GetFilesDataResponseById(filesDataId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid filesDataId) =>
              await _applicationClientConverting.SetFilesDataLoadedByClient(filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id) => await _applicationClientConverting.AbortConvertingById(id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => await _applicationClientConverting.GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() => await _applicationClientConverting.GetSignaturesDepartments();
    }
}
