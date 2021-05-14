using System;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Base.Components;
using GadzhiDAL.Entities.PaperSizes;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public static class ConverterFilesDataEntitiesFromDtoServer
    {
        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        public static PackageDataEntity UpdatePackageDataFromShortResponse(PackageDataEntity packageDataEntity,
                                                                           PackageDataShortResponseServer packageDataShortResponse)
        {
            packageDataEntity.StatusProcessingProject = packageDataShortResponse.StatusProcessingProject;

            var filesDataIntermediateEntity = packageDataEntity.FileDataEntities.
                                              Join(packageDataShortResponse.FilesData,
                                                   fileEntity => fileEntity.FilePath,
                                                   filesIntermediateResponse => filesIntermediateResponse.FilePath,
                                                   UpdateFileDataFromShortResponse).
                                              ToList();
            packageDataEntity.SetFileDataEntities(filesDataIntermediateEntity);

            return packageDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        public static PackageDataEntity UpdatePackageDataFromResponse(PackageDataEntity packageDataEntity,
                                                                      PackageDataResponseServer packageDataResponse)
        {
            if (CheckStatusProcessing.CompletedStatusProcessingProject.
                Contains(packageDataEntity.StatusProcessingProject)) return packageDataEntity;

            packageDataEntity.StatusProcessingProject = packageDataResponse.StatusProcessingProject;

            var fileDataEntities = packageDataEntity.FileDataEntities.
                                   Join(packageDataResponse.FilesData,
                                   fileEntity => fileEntity.FilePath,
                                   fileResponse => fileResponse.FilePath,
                                   UpdateFileDataFromResponse).
                                   ToList();
            packageDataEntity.SetFileDataEntities(fileDataEntities);

            return packageDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа с данными
        /// </summary>      
        public static void UpdateFileDataFromIntermediateResponse(PackageDataEntity packageDataEntity,
                                                                  FileDataResponseServer fileDataResponseServer)
        {
            if (CheckStatusProcessing.CompletedStatusProcessingProject.
                Contains(packageDataEntity.StatusProcessingProject)) return;

            var fileDataEntity = packageDataEntity.FileDataEntities.First(fileData => fileData.FilePath == fileDataResponseServer.FilePath);
            UpdateFileDataFromResponse(fileDataEntity, fileDataResponseServer);
        }

        /// <summary>
        /// Обновить модель файла данных на основе промежуточного ответа
        /// </summary>      
        public static FileDataEntity UpdateFileDataFromShortResponse(FileDataEntity fileDataEntity, FileDataShortResponseServer fileDataResponse)
        {
            fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
            if (fileDataEntity.FileDataSource.Count == 0)
            {
                var errorCommonResponse = new ErrorCommonResponse
                {
                    ErrorConvertingType = ErrorConvertingType.IncorrectDataSource,
                    ErrorDescription = "Ошибка загрузки данных на сервер",
                };
                fileDataResponse.FileErrors.Add(errorCommonResponse);
            }
            fileDataEntity.FileErrors = fileDataResponse.FileErrors.Select(ToErrorComponent).ToList();
            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataEntity UpdateFileDataFromResponse(FileDataEntity fileDataEntity, FileDataResponseServer fileDataResponse)
        {
            var fileDataSourceEntity = fileDataResponse.FilesDataSource?.Select(ToFileDataSource);
            fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
            fileDataEntity.FileErrors = fileDataResponse.FileErrors.Select(ToErrorComponent).ToList();
            fileDataEntity.SetFileDataSourceEntities(fileDataSourceEntity);

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataSourceEntity ToFileDataSource(FileDataSourceResponseServer fileDataSourceResponseServer) =>
            new FileDataSourceEntity
            {
                FileName = fileDataSourceResponseServer.FileName,
                FileExtensionType = fileDataSourceResponseServer.FileExtensionType,
                FileDataSource = fileDataSourceResponseServer.FileDataSource,
                PaperSizes = fileDataSourceResponseServer.PaperSizes.ToList(),
                PrinterName = fileDataSourceResponseServer.PrinterName,
            };

        /// <summary>
        /// Преобразовать ошибки в формат БД
        /// </summary>
        public static ErrorComponent ToErrorComponent(ErrorCommonResponse error) =>
            new ErrorComponent
            {
                ErrorConvertingType = error.ErrorConvertingType,
                ErrorDescription = error.ErrorDescription,
            };
    }
}
