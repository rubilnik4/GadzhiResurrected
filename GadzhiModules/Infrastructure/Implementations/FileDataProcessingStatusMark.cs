using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для получения файлов, у которых необходимо изменить статус
    /// </summary>
    public class FileDataProcessingStatusMark : IFileDataProcessingStatusMark
    {
        public FileDataProcessingStatusMark(IPackageData packageInfoProject,
                                            IProjectSettings projectSettings,
                                            IConverterClientPackageDataToDto converterClientPackageDataToDto,
                                            IConverterClientPackageDataFromDto converterClientPackageDataFromDto)
        {
            _packageInfoProject = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _converterClientPackageDataToDto = converterClientPackageDataToDto ?? throw new ArgumentNullException(nameof(converterClientPackageDataToDto));
            _converterClientPackageDataFromDto = converterClientPackageDataFromDto ?? throw new ArgumentNullException(nameof(converterClientPackageDataFromDto));
        }

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageInfoProject;

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Конвертеры из локальной модели в трансферную
        /// </summary>  
        private readonly IConverterClientPackageDataToDto _converterClientPackageDataToDto;

        /// <summary>
        /// Конвертеры из трансферной модели в локальную
        /// </summary>  
        private readonly IConverterClientPackageDataFromDto _converterClientPackageDataFromDto;

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<IResultValue<PackageDataRequestClient>> GetFilesDataToRequest() =>
            await _converterClientPackageDataToDto.ToPackageDataRequest(_packageInfoProject, _projectSettings.ConvertingSettings);

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        public PackageStatus GetFilesInSending()
        {
            var filesSending = _packageInfoProject?.FilesData?.
                               Select(file => new FileStatus(file.FilePath, StatusProcessing.Sending));

            return new PackageStatus(filesSending, StatusProcessingProject.Sending);
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        public PackageStatus GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = _packageInfoProject?.FilesDataPath.
                                Where(filePath => fileDataRequestPaths?.Contains(filePath) == false).
                                Select(filePath => new FileStatus(filePath, StatusProcessing.End,
                                                                  new ErrorCommon(ErrorConvertingType.FileNotFound, $"Файл не найден {filePath}")));

            return new PackageStatus(filesNotFound, StatusProcessingProject.Sending);
        }

        /// <summary>
        /// Поменять статус файлов после промежуточного отчета
        /// </summary>       
        public PackageStatus GetPackageStatusIntermediateResponse(PackageDataShortResponseClient packageDataShortResponse) =>
            _converterClientPackageDataFromDto.ToPackageStatusFromIntermediateResponse(packageDataShortResponse);
             
        /// <summary>
        /// Поменять статус файла после перед записью
        /// </summary>       
        public FileStatus GetFileStatusCompleteResponseBeforeWriting(FileDataResponseClient fileDataResponseClient) =>
            ConverterClientPackageDataFromDto.ConvertToFileStatusFromResponse(fileDataResponseClient);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        public PackageStatus GetFilesStatusCompleteResponseBeforeWriting(PackageDataResponseClient packageDataResponse) =>
            _converterClientPackageDataFromDto.ToPackageStatus(packageDataResponse);

        /// <summary>
        /// Поменять статус файла после записи
        /// </summary>       
        public async Task<FileStatus> GetFileStatusCompleteResponseAndWritten(FileDataResponseClient fileDataResponseClient) =>
            await _converterClientPackageDataFromDto.ToFileStatusFromResponseAndSaveFile(fileDataResponseClient);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и записи файлов
        /// </summary>       
        public async Task<PackageStatus> GetFilesStatusCompleteResponseAndWritten(PackageDataResponseClient packageDataResponse) =>
            await _converterClientPackageDataFromDto.ToFilesStatusAndSaveFiles(packageDataResponse);

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        public PackageStatus GetPackageStatusAfterSend(PackageDataRequestClient packageDataRequest, PackageDataShortResponseClient packageDataShortResponse)
        {
            var filesNotFound = GetFilesNotFound(packageDataRequest?.FilesData);
            var filesChangedStatus = GetPackageStatusIntermediateResponse(packageDataShortResponse);

            var filesDataUnion = filesNotFound.FileStatus.UnionNotNull(filesChangedStatus.FileStatus);
            return new PackageStatus(filesDataUnion,
                                     packageDataShortResponse?.StatusProcessingProject ?? StatusProcessingProject.Sending,
                                     filesChangedStatus.QueueStatus);
        }
    }
}
