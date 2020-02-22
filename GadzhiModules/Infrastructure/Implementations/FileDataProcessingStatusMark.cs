using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IFilesData _filesInfoProject;

        /// <summary>
        /// Конвертеры из локальной модели в трансферную
        /// </summary>  
        private readonly IConverterClientFilesDataToDTO _converterClientFilesDataToDTO;

        /// <summary>
        /// Конвертеры из трансферной модели в локальную
        /// </summary>  
        private readonly IConverterClientFilesDataFromDTO _converterClientFilesDataFromDTO;

        public FileDataProcessingStatusMark(IFilesData filesInfoProject,
                                            IConverterClientFilesDataToDTO converterClientFilesDataToDTO,
                                            IConverterClientFilesDataFromDTO converterClientFilesDataFromDTO)
        {
            _filesInfoProject = filesInfoProject;
            _converterClientFilesDataToDTO = converterClientFilesDataToDTO;
            _converterClientFilesDataFromDTO = converterClientFilesDataFromDTO;
        }

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<FilesDataRequestClient> GetFilesDataToRequest()
        {
            return await _converterClientFilesDataToDTO.ConvertToFilesDataRequest(_filesInfoProject);
        }

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        public Task<FilesStatus> GetFilesInSending()
        {
            var filesInSending = _filesInfoProject?.
                                 FilesInfo?.
                                 Select(file => new FileStatus(file.FilePath,
                                                               StatusProcessing.Sending,
                                                               FileConvertErrorType.IncorrectFileName));

            var filesStatusInSendind = new FilesStatus(filesInSending,
                                                       StatusProcessingProject.Sending);

            return Task.FromResult(filesStatusInSendind);
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        public Task<FilesStatus> GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = _filesInfoProject?.
                                FilesInfoPath.
                                Where(filePath => fileDataRequestPaths?.Contains(filePath) == false).
                                Select(filePath => new FileStatus(filePath,
                                                                  StatusProcessing.Error,
                                                                  FileConvertErrorType.FileNotFound));
            var filesStatusInSendind = new FilesStatus(filesNotFound,
                                                       StatusProcessingProject.Sending);
            return Task.FromResult(filesStatusInSendind);
        }

        /// <summary>
        /// Поменять статус файлов после промежуточного отчета
        /// </summary>       
        public Task<FilesStatus> GetFilesStatusIntermediateResponse(FilesDataIntermediateResponseClient filesDataIntermediateResponse)
        {
            var filesStatusIntermediate = _converterClientFilesDataFromDTO.
                                          ConvertToFilesStatusFromIntermediateResponse(filesDataIntermediateResponse);

            return Task.FromResult(filesStatusIntermediate);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        public Task<FilesStatus> GetFilesStatusCompleteResponseBeforeWriting(FilesDataResponseClient filesDataResponse)
        {
            var filesStatusResponseBeforeWriting = _converterClientFilesDataFromDTO.
                                                   ConvertToFilesStatus(filesDataResponse);
            return Task.FromResult(filesStatusResponseBeforeWriting);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и записи файлов
        /// </summary>       
        public async Task<FilesStatus> GetFilesStatusCompleteResponseAndWritten(FilesDataResponseClient filesDataResponse)
        {
            var filesStatusResponse = await _converterClientFilesDataFromDTO.
                                            ConvertToFilesStatusAndSaveFiles(filesDataResponse);
            return filesStatusResponse;
        }

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        public async Task<FilesStatus> GetFilesStatusUnionAfterSendAndNotFound(FilesDataRequestClient filesDataRequest,
                                                                               FilesDataIntermediateResponseClient filesDataIntermediateResponse)
        {
            var filesNotFound = GetFilesNotFound(filesDataRequest?.FilesData);
            var filesChangedStatus = GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            await Task.WhenAll(filesNotFound, filesChangedStatus);

            var filesDataUnion = filesNotFound?.Result.FileStatus.Union(filesChangedStatus.Result.FileStatus);
            var filesStatusUnion = new FilesStatus(filesDataUnion,
                                                   filesDataIntermediateResponse?.StatusProcessingProject ?? StatusProcessingProject.Sending,
                                                   filesChangedStatus.Result.FilesQueueStatus);
            return filesStatusUnion;
        }
    }
}
