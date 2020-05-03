using System;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public static PackageDataEntity UpdatePackageDataFromIntermediateResponse(PackageDataEntity packageDataEntity,
                                                                                  PackageDataIntermediateResponseServer packageDataIntermediateResponse)
        {
            if (packageDataEntity == null) throw new ArgumentNullException(nameof(packageDataEntity));
            if (packageDataIntermediateResponse == null) throw new ArgumentNullException(nameof(packageDataIntermediateResponse));

            packageDataEntity.StatusProcessingProject = packageDataIntermediateResponse.StatusProcessingProject;

            var filesDataIntermediateEntity = packageDataEntity.FileDataEntities?.
                                              Join(packageDataIntermediateResponse.FilesData,
                                              fileEntity => fileEntity.FilePath,
                                              filesIntermediateResponse => filesIntermediateResponse.FilePath,
                                              UpdateFileDataFromIntermediateResponse).
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
            if (packageDataEntity == null) throw new ArgumentNullException(nameof(packageDataEntity));
            if (packageDataResponse == null) throw new ArgumentNullException(nameof(packageDataResponse));

            if (CheckStatusProcessing.CompletedStatusProcessingProject.
                Contains(packageDataEntity.StatusProcessingProject)) return packageDataEntity;

            packageDataEntity.StatusProcessingProject = packageDataResponse.StatusProcessingProject;

            var fileDataEntities = packageDataEntity.FileDataEntities?.
                                   Join(packageDataResponse.FilesData,
                                   fileEntity => fileEntity.FilePath,
                                   fileResponse => fileResponse.FilePath,
                                   UpdateFileDataFromResponse).
                                   ToList();
            packageDataEntity.SetFileDataEntities(fileDataEntities);

            return packageDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе промежуточного ответа
        /// </summary>      
        public static FileDataEntity UpdateFileDataFromIntermediateResponse(FileDataEntity fileDataEntity,
                                                                                  FileDataIntermediateResponseServer fileDataResponse)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));
            if (fileDataResponse == null) throw new ArgumentNullException(nameof(fileDataResponse));

            fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
            fileDataEntity.FileConvertErrorType = fileDataResponse.FileConvertErrorTypes.ToList();

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataEntity UpdateFileDataFromResponse(FileDataEntity fileDataEntity,
                                                                      FileDataResponseServer fileDataResponse)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));
            if (fileDataResponse == null) throw new ArgumentNullException(nameof(fileDataResponse));

            var fileDataSourceEntity = fileDataResponse.FilesDataSource?.AsQueryable().
                                       Select(fileData => ToFileDataSource(fileData));
            fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
            fileDataEntity.FileConvertErrorType = fileDataResponse.FileConvertErrorTypes.ToList();
            fileDataEntity.SetFileDataSourceEntities(fileDataSourceEntity);

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public static FileDataSourceEntity ToFileDataSource(FileDataSourceResponseServer fileDataSourceResponseServer) =>
            new FileDataSourceEntity()
            {
                FileName = fileDataSourceResponseServer?.FileName ?? throw new ArgumentNullException(nameof(fileDataSourceResponseServer)),
                FileDataSource = fileDataSourceResponseServer.FileDataSource,
                PaperSize = fileDataSourceResponseServer.PaperSize,
                PrinterName = fileDataSourceResponseServer.PrinterName,
            };
    }
}
