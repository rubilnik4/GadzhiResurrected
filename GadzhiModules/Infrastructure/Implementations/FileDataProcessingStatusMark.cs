﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;
using GadzhiModules.Modules.FilesConvertModule.Models.Interfaces;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для получения файлов, у которых необходимо изменить статус
    /// </summary>
    public class FileDataProcessingStatusMark : IFileDataProcessingStatusMark
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageInfoProject;

        /// <summary>
        /// Конвертеры из локальной модели в трансферную
        /// </summary>  
        private readonly IConverterClientPackageDataToDto _converterClientPackageDataToDto;

        /// <summary>
        /// Конвертеры из трансферной модели в локальную
        /// </summary>  
        private readonly IConverterClientPackageDataFromDto _converterClientPackageDataFromDto;

        public FileDataProcessingStatusMark(IPackageData packageInfoProject,
                                            IConverterClientPackageDataToDto converterClientPackageDataToDto,
                                            IConverterClientPackageDataFromDto converterClientPackageDataFromDto)
        {
            _packageInfoProject = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _converterClientPackageDataToDto = converterClientPackageDataToDto ?? throw new ArgumentNullException(nameof(converterClientPackageDataToDto));
            _converterClientPackageDataFromDto = converterClientPackageDataFromDto ?? throw new ArgumentNullException(nameof(converterClientPackageDataFromDto));
        }

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<PackageDataRequestClient> GetFilesDataToRequest() =>
            await _converterClientPackageDataToDto.ToPackageDataRequest(_packageInfoProject);

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        public Task<PackageStatus> GetFilesInSending()
        {
            var filesSending = _packageInfoProject?.FilesData?.
                               Select(file => new FileStatus(file.FilePath, StatusProcessing.Sending, FileConvertErrorType.IncorrectFileName));

            var filesStatusInSending = new PackageStatus(filesSending, StatusProcessingProject.Sending);
            return Task.FromResult(filesStatusInSending);
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        public Task<PackageStatus> GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = _packageInfoProject?.FilesDataPath.
                                Where(filePath => fileDataRequestPaths?.Contains(filePath) == false).
                                Select(filePath => new FileStatus(filePath, StatusProcessing.End, FileConvertErrorType.FileNotFound));

            var filesStatusInSending = new PackageStatus(filesNotFound, StatusProcessingProject.Sending);
            return Task.FromResult(filesStatusInSending);
        }

        /// <summary>
        /// Поменять статус файлов после промежуточного отчета
        /// </summary>       
        public Task<PackageStatus> GetPackageStatusIntermediateResponse(PackageDataIntermediateResponseClient packageDataIntermediateResponse)
        {
            var filesStatusIntermediate = _converterClientPackageDataFromDto.ToPackageStatusFromIntermediateResponse(packageDataIntermediateResponse);
            return Task.FromResult(filesStatusIntermediate);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        public Task<PackageStatus> GetFilesStatusCompleteResponseBeforeWriting(PackageDataResponseClient packageDataResponse)
        {
            var filesStatusResponseBeforeWriting = _converterClientPackageDataFromDto.ToPackageStatus(packageDataResponse);
            return Task.FromResult(filesStatusResponseBeforeWriting);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и записи файлов
        /// </summary>       
        public async Task<PackageStatus> GetFilesStatusCompleteResponseAndWritten(PackageDataResponseClient packageDataResponse) =>
            await _converterClientPackageDataFromDto.ToFilesStatusAndSaveFiles(packageDataResponse);

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        public async Task<PackageStatus> GetPackageStatusAfterSend(PackageDataRequestClient packageDataRequest,
                                                                   PackageDataIntermediateResponseClient packageDataIntermediateResponse)
        {
            var filesNotFound = GetFilesNotFound(packageDataRequest?.FilesData);
            var filesChangedStatus = GetPackageStatusIntermediateResponse(packageDataIntermediateResponse);
            await Task.WhenAll(filesNotFound, filesChangedStatus);

            var filesDataUnion = filesNotFound.Result.FileStatus.UnionNotNull(filesChangedStatus.Result.FileStatus);
            return new PackageStatus(filesDataUnion,
                                     packageDataIntermediateResponse?.StatusProcessingProject ?? StatusProcessingProject.Sending,
                                     filesChangedStatus.Result.QueueStatus);
        }
    }
}
