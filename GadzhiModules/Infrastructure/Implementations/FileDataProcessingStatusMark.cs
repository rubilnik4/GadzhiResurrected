using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
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
        /// Конвертеры из локальной модели в трансферную
        /// </summary>  
        IConverterClientFilesDataToDTO ConverterClientFilesDataToDTO { get; }

        /// <summary>
        /// Конвертеры из трансферной модели в локальную
        /// </summary>  
        IConverterClientFilesDataFromDTO ConverterClientFilesDataFromDTO { get; }

        public FileDataProcessingStatusMark(IFilesData filesInfoProject,                                           
                                            IConverterClientFilesDataToDTO converterClientFilesDataToDTO,
                                            IConverterClientFilesDataFromDTO converterClientFilesDataFromDTO)
        {
            FilesInfoProject = filesInfoProject;        
            ConverterClientFilesDataToDTO = converterClientFilesDataToDTO;
            ConverterClientFilesDataFromDTO = converterClientFilesDataFromDTO;
        }

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<FilesDataRequest> GetFilesDataToRequest()
        {
            return await ConverterClientFilesDataToDTO.ConvertToFilesDataRequest(FilesInfoProject);
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
            var filesStatusIntermediate = ConverterClientFilesDataFromDTO.
                                          ConvertToFilesStatusFromIntermediateResponse(filesDataIntermediateResponse);

            return Task.FromResult(filesStatusIntermediate);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        public Task<FilesStatus> GetFilesStatusCompleteResponseBeforeWriting(FilesDataResponse filesDataResponse)
        {
            var filesStatusResponseBeforeWriting = ConverterClientFilesDataFromDTO.
                                                   ConvertToFilesStatus(filesDataResponse);
            return Task.FromResult(filesStatusResponseBeforeWriting);
        }

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и записи файлов
        /// </summary>       
        public async Task<FilesStatus> GetFilesStatusCompleteResponseAndWritten(FilesDataResponse filesDataResponse)
        {
            var filesStatusResponse = await ConverterClientFilesDataFromDTO.
                                            ConvertToFilesStatusAndSaveFiles(filesDataResponse);
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
                                                   filesChangedStatus.Result.FilesQueueStatus);
            return filesStatusUnion;
        }      
    }
}
