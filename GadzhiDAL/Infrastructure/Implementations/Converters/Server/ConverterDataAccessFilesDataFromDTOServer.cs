using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataFromDTOServer : IConverterDataAccessFilesDataFromDtoServer
    {
        public ConverterDataAccessFilesDataFromDTOServer()
        {

        }

        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                             PackageDataIntermediateResponseServer packageDataIntermediateResponse)
        {
            if (filesDataEntity != null && packageDataIntermediateResponse != null)
            {
                filesDataEntity.StatusProcessingProject = packageDataIntermediateResponse.StatusProcessingProject;

                var filesDataIntermediateEntity = filesDataEntity.FileDataEntities?.
                                                  Join(packageDataIntermediateResponse?.FilesData,
                                                  fileEntity => fileEntity.FilePath,
                                                  filesIntermediateResponse => filesIntermediateResponse.FilePath,
                                                  (fileEntity, fileIntermediateResponse) => UpdateFileDataAccessFromIntermediateResponse(fileEntity, fileIntermediateResponse)).
                                                  ToList();
            }
            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                                 PackageDataResponseServer packageDataResponse)
        {
            if (filesDataEntity != null && packageDataResponse != null &&
                !CheckStatusProcessing.CompletedStatusProcessingProject.Contains(filesDataEntity.StatusProcessingProject))
            {
                filesDataEntity.StatusProcessingProject = packageDataResponse.StatusProcessingProject;

                filesDataEntity.FileDataEntities?.Join(packageDataResponse?.FilesData,
                                                fileEntity => fileEntity.FilePath,
                                                fileResponse => fileResponse.FilePath,
                                                (fileEntity, fileResponse) => UpdateFileDataAccessFromResponse(fileEntity, fileResponse)).
                                                ToList();

            }
            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе промежуточного ответа
        /// </summary>      
        public FileDataEntity UpdateFileDataAccessFromIntermediateResponse(FileDataEntity fileDataEntity,
                                                                           FileDataIntermediateResponseServer fileDataResponse)
        {
            if (fileDataEntity != null && fileDataResponse != null)
            {
                fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
                fileDataEntity.FileConvertErrorType = fileDataResponse.FileConvertErrorTypes.ToList();
            }

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public FileDataEntity UpdateFileDataAccessFromResponse(FileDataEntity fileDataEntity,
                                                               FileDataResponseServer fileDataResponse)
        {
            if (fileDataEntity != null && fileDataResponse != null)
            {
                var fileDataSourceEntity = fileDataResponse.FilesDataSourceResponseServer?.AsQueryable().
                                           Select(fileData => ToFileDataSourceAccess(fileData));
                fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
                fileDataEntity.FileConvertErrorType = fileDataResponse.FileConvertErrorTypes.ToList();
                fileDataEntity.SetFileDataSourceEntities(fileDataSourceEntity);
            }

            return fileDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе окончательного ответа
        /// </summary>      
        public FileDataSourceEntity ToFileDataSourceAccess(FileDataSourceResponseServer fileDataSourceResponseServer) =>
            new FileDataSourceEntity()
            {
                FileName = fileDataSourceResponseServer?.FileName,
                FileDataSource = fileDataSourceResponseServer?.FileDataSource,
                PaperSize = fileDataSourceResponseServer?.PaperSize,
                PrinterName = fileDataSourceResponseServer?.PrinterName,
            };
    }
}
