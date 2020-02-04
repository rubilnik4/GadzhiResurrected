using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Helpers.Converters.DTO;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IFilesData FilesInfoProject { get; }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }

        public FileDataProcessingStatusMark(IFilesData filesInfoProject,
                                            IFileSystemOperations fileSystemOperations)
        {
            FilesInfoProject = filesInfoProject;
            FileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<FilesDataRequest> GetFilesDataToRequest()
        {
            return await FilesDataClientToDTOConverter.ConvertToFilesDataRequest(FilesInfoProject, FileSystemOperations);
        }

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        public Task<FilesStatus> GetFilesInSending()
        {
            var filesInSending = FilesInfoProject?.
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
        public Task<FilesStatus> GetFilesNotFound(IEnumerable<FileDataRequest> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = FilesInfoProject?.
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
        public Task<FilesStatus> GetFilesStatusIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            var filesStatusIntermediate = FilesDataFromDTOConverterToClient.
                                             ConvertToFilesStatusFromIntermediateResponse(filesDataIntermediateResponse);

            return Task.FromResult(filesStatusIntermediate);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        public Task<FilesStatus> GetFilesStatusCompleteResponseBeforeWriting(FilesDataResponse filesDataResponse)
        {
            var filesStatusResponseBeforeWriting = FilesDataFromDTOConverterToClient.
                                                   ConvertToFilesStatus(filesDataResponse);
            return Task.FromResult(filesStatusResponseBeforeWriting);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и записи файлов
        /// </summary>       
        public async Task<FilesStatus> GetFilesStatusCompleteResponseAndWritten(FilesDataResponse filesDataResponse)
        {
            var filesStatusResponse = await FilesDataFromDTOConverterToClient.
                                            ConvertToFilesStatusAndSaveFiles(filesDataResponse,
                                                                             FileSystemOperations);
            return filesStatusResponse;
        }

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        public async Task<FilesStatus> GetFilesStatusUnionAfterSendAndNotFound(FilesDataRequest filesDataRequest,
                                                                               FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            var filesNotFound = GetFilesNotFound(filesDataRequest.FilesData);
            var filesChangedStatus = GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            await Task.WhenAll(filesNotFound, filesChangedStatus);

            var filesDataUnion = filesNotFound?.Result.FileStatus.Union(filesChangedStatus.Result.FileStatus);
            var filesStatusUnion = new FilesStatus(filesDataUnion,
                                                   filesDataIntermediateResponse.StatusProcessingProject,
                                                   filesChangedStatus.Result.FilesQueueInfo);
            return filesStatusUnion;
        }      
    }
}
