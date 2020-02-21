using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Linq;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataToDTOServer : IConverterDataAccessFilesDataToDTOServer
    {
        public ConverterDataAccessFilesDataToDTOServer()
        {

        }

        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public FilesDataRequestServer ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataRequestServer()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    AttemptingConvertCount = filesDataEntity.IdentityMachine.AttemptingConvertCount,
                    FilesData = filesDataEntity.FilesData?.
                                                Select(fileData => ConvertFileDataAccessToRequest(fileData)).
                                                ToList(),
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в запрос
        /// </summary>
        private FileDataRequestServer ConvertFileDataAccessToRequest(FileDataEntity fileDataEntity)
        {
            return new FileDataRequestServer()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileDataSource = fileDataEntity.FileDataSource,
            };
        }
    }
}
