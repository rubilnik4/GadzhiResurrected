using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Helpers.Converters.DTO;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
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
        private IFileSeach FileSeach { get; }

        public FileDataProcessingStatusMark(IFilesData filesInfoProject,
                                            IFileSeach fileSeach)
        {
            FilesInfoProject = filesInfoProject;
            FileSeach = fileSeach;
        }

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        public async Task<IEnumerable<FileDataRequest>> GetFilesToRequest()
        {
            var filesRequestExist = await Task.WhenAll(FilesInfoProject?.FilesInfo?.Where(file => FileSeach.IsFileExist(file.FilePath))?.
                                                                                    Select(file => FilesDataToDTOConverter.ConvertToFileDataDTO(file, FileSeach)));
            var filesRequestEnsuredWithBytes = filesRequestExist?.Where(file => file.FileDataSource != null);

            return filesRequestEnsuredWithBytes;
        }

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        public Task<IEnumerable<FileStatus>> GetFilesInSending()
        {
            var filesInSending = FilesInfoProject?.
                                 FilesInfo?.
                                 Select(file => new FileStatus(file.FilePath, StatusProcessing.Sending));

            return Task.FromResult(filesInSending);
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        public Task<IEnumerable<FileStatus>> GetFilesNotFound(IEnumerable<FileDataRequest> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = FilesInfoProject?.
                                FilesInfoPath.
                                Where(filePath => fileDataRequestPaths?.Contains(filePath) == false).
                                Select(filePath => new FileStatus(filePath, StatusProcessing.Error, FileConvertErrorType.FileNotFound));

            return Task.FromResult(filesNotFound);
        }

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        public Task<IEnumerable<FileStatus>> GetFilesStatusAfterUpload(FilesDataIntermediateResponse fileDataResponse)
        {
            var filesStatusAfterUpload = fileDataResponse?.
                                         FilesData.
                                         Select(fileResponse => FilesDataFromDTOConverter.ConvertToFileStatus(fileResponse));

            return Task.FromResult(filesStatusAfterUpload);
        }
    }
}
