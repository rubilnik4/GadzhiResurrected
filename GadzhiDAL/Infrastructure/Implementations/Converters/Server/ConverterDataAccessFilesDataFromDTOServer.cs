using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataFromDTOServer : IConverterDataAccessFilesDataFromDTOServer
    {
        public ConverterDataAccessFilesDataFromDTOServer()
        {

        }

        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                             FilesDataIntermediateResponseServer filesDataIntermediateResponse)
        {
            if (filesDataEntity != null && filesDataIntermediateResponse != null)
            {
                filesDataEntity.IsCompleted = filesDataIntermediateResponse.IsCompleted;
                filesDataEntity.StatusProcessingProject = filesDataIntermediateResponse.StatusProcessingProject;

                var filesDataIntermediateEntity = filesDataEntity.FilesData?.
                                                  Join(filesDataIntermediateResponse?.FilesData,
                                                  fileEntity => fileEntity.FilePath,
                                                  filesIntermediateResponse => filesIntermediateResponse.FilePath,
                                                  (fileEntity, fileIntermediateResponse) => UpdateFileDataAccessFromIntermediateResponse(fileEntity, fileIntermediateResponse));
            }
            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        public FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                                             FilesDataResponseServer filesDataResponse)
        {
            if (filesDataEntity != null && filesDataResponse != null &&
                !filesDataEntity.IsCompleted)
            {
                filesDataEntity.IsCompleted = filesDataResponse.IsCompleted;
                filesDataEntity.StatusProcessingProject = filesDataResponse.StatusProcessingProject;

                filesDataEntity.FilesData?.Join(filesDataResponse?.FilesData,
                                                fileEntity => fileEntity.FilePath,
                                                fileResponse => fileResponse.FilePath,
                                                (fileEntity, fileResponse) => UpdateFileDataAccessFromResponse(fileEntity, fileResponse));

            }
            return filesDataEntity;
        }

        /// <summary>
        /// Обновить модель файла данных на основе промежуточного ответа
        /// </summary>      
        public FileDataEntity UpdateFileDataAccessFromIntermediateResponse(FileDataEntity fileDataEntity,
                                                                                 FileDataIntermediateResponseServer fileDataIntermediateResponse)
        {
            if (fileDataEntity != null && fileDataIntermediateResponse != null)
            {
                fileDataEntity.IsCompleted = fileDataIntermediateResponse.IsCompleted;
                fileDataEntity.StatusProcessing = fileDataIntermediateResponse.StatusProcessing;
                fileDataEntity.FileConvertErrorType = fileDataIntermediateResponse.FileConvertErrorType.ToList();
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
                var fileDataSourceEntity = fileDataResponse.FileDataSourceResponseServer?.AsQueryable().
                                           Select(fileData => ToFileDataSourceAccess(fileData));

                fileDataEntity.IsCompleted = fileDataResponse.IsCompleted;
                fileDataEntity.StatusProcessing = fileDataResponse.StatusProcessing;
                fileDataEntity.FileConvertErrorType = fileDataResponse.FileConvertErrorType.ToList();
                fileDataEntity.SetFileDataSourceEntity(fileDataSourceEntity);
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
            };
    }
}
