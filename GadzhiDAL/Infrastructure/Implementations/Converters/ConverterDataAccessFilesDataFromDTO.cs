using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDTO.Helpers;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataFromDTO : IConverterDataAccessFilesDataFromDTO
    {
        public ConverterDataAccessFilesDataFromDTO()
        {

        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в модель базы данных
        /// </summary>      
        public FilesDataEntity ConvertToFilesDataAccess(FilesDataRequest filesDataRequest)
        {
            var filesDataAccessToConvert = filesDataRequest?.FilesData?.Select(fileDTO =>
                                               ConvertToFileDataAccess(fileDTO));

            var filesDataEntity = new FilesDataEntity();
            filesDataEntity.SetId(filesDataRequest.Id);
            filesDataEntity.IdentityMachine.IdentityLocalName = filesDataRequest.IdentityName;
            filesDataEntity.SetFilesData(filesDataAccessToConvert);

            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                             FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            if (filesDataEntity != null && filesDataIntermediateResponse != null)
            {
                filesDataEntity.IsCompleted = filesDataIntermediateResponse.IsCompleted;
                filesDataEntity.StatusProcessingProject = filesDataIntermediateResponse.StatusProcessingProject;

                foreach (var fileDataAccess in filesDataEntity.FilesData)
                {
                    FileDataIntermediateResponse fileDataIntermediate = filesDataIntermediateResponse.FilesData?.
                        FirstOrDefault(fileDataInter => fileDataInter.FilePath == fileDataAccess.FilePath);

                    UpdateFileDataAccessFromIntermediateResponse(fileDataAccess,
                                                                 fileDataIntermediate);
                }
            }
            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                                         FilesDataResponse filesDataResponse)
        {
            if (filesDataEntity != null && filesDataResponse != null)
            {
                filesDataEntity.IsCompleted = filesDataResponse.IsCompleted;
                filesDataEntity.StatusProcessingProject = filesDataResponse.StatusProcessingProject;
             
                foreach (var fileDataAccess in filesDataEntity.FilesData)
                {
                    FileDataResponse fileData = filesDataResponse.FilesData?.
                        FirstOrDefault(fileDataInter => fileDataInter.FilePath == fileDataAccess.FilePath);

                    UpdateFileDataAccessFromResponse(fileDataAccess,
                                                     fileData);
                }
            }
            return filesDataEntity;
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private FileDataEntity ConvertToFileDataAccess(FileDataRequest fileDataRequest)
        {
            var fileDataEntity = new FileDataEntity()
            {
                ColorPrint = fileDataRequest.ColorPrint,
                FilePath = fileDataRequest.FilePath,
                FileDataSource = fileDataRequest.FileDataSource,
            };

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе промежуточного ответа
        /// </summary>      
        public FileDataEntity UpdateFileDataAccessFromIntermediateResponse(FileDataEntity fileDataEntity,
                                                                             FileDataIntermediateResponse fileDataIntermediateResponse)
        {
            if (fileDataEntity != null && fileDataIntermediateResponse != null)
            {
                fileDataEntity.IsCompleted = fileDataIntermediateResponse.IsCompleted;
                fileDataEntity.StatusProcessing = fileDataIntermediateResponse.StatusProcessing;
                fileDataEntity.SetFileConvertErrorType(fileDataIntermediateResponse.FileConvertErrorType);
            }

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public FileDataEntity UpdateFileDataAccessFromResponse(FileDataEntity fileDataEntity,
                                                               FileDataResponse fileDataResponse)
        {
            if (fileDataEntity != null && fileDataResponse != null)
            {
                fileDataEntity.IsCompleted = fileDataResponse.IsCompleted;
                fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
                fileDataEntity.FileDataSource = fileDataResponse.FileDataSource;
                fileDataEntity.SetFileConvertErrorType(fileDataResponse.FileConvertErrorType);               
            }

            return fileDataEntity;
        }
    }
}
