using System;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Components;
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
            packageDataEntity.FileDataEntities = filesDataIntermediateEntity;

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
            packageDataEntity.FileDataEntities = fileDataEntities;
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
            fileDataEntity.FileErrors = fileDataResponse.FileErrors.Select(ToErrorComponent).ToList();
            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataEntity UpdateFileDataFromResponse(FileDataEntity fileDataEntity, FileDataResponseServer fileDataResponse)
        {
            var fileDataSourceEntity = fileDataResponse.FilesDataSource.
                                       Select(fileData => ToFileDataSource(fileData, fileDataEntity)).
                                       ToList();
            fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
            fileDataEntity.FileErrors = fileDataResponse.FileErrors.Select(ToErrorComponent).ToList();
            fileDataEntity.FileDataSourceServerEntities = fileDataSourceEntity;
            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataSourceEntity ToFileDataSource(FileDataSourceResponseServer fileDataSource,
                                                            FileDataEntity fileDataEntity) =>
            new FileDataSourceEntity(fileDataSource.FileName, fileDataSource.FileExtensionType,
                                     fileDataSource.PaperSize, fileDataSource.PrinterName, fileDataSource.FileDataSource).
            Void(source => source.FileDataEntity = fileDataEntity);

        /// <summary>
        /// Преобразовать ошибки в формат БД
        /// </summary>
        public static ErrorComponent ToErrorComponent(ErrorCommonResponse error) =>
            new ErrorComponent(error.ErrorConvertingType, error.Description);
    }
}
