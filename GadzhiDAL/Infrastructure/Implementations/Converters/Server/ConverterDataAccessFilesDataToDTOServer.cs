using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataToDTOServer : IConverterDataAccessFilesDataToDtoServer
    {
        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public async Task<PackageDataRequestServer> ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                var filesData = await filesDataEntity.FileDataEntities?.AsQueryable().
                                      Select(fileData => ConvertFileDataAccessToRequest(fileData)).ToListAsync();              
                return new PackageDataRequestServer()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    AttemptingConvertCount = filesDataEntity.AttemptingConvertCount,
                    FilesData = filesData,
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
                FileDataSource = fileDataEntity.FileDataSourceClient.AsQueryable().ToList(),
            };
        }
    }
}
