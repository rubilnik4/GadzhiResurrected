using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Client
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public class ConverterDataAccessFilesDataFromDTOClient : IConverterDataAccessFilesDataFromDTOClient
    {
        public ConverterDataAccessFilesDataFromDTOClient()
        {

        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в модель базы данных
        /// </summary>      
        public FilesDataEntity ConvertToFilesDataAccess(FilesDataRequestClient filesDataRequest)
        {
            if (filesDataRequest != null)
            {
                var filesDataAccessToConvert = filesDataRequest?.FileDatas?.AsQueryable().
                                               Select(fileDTO => ConvertToFileDataAccess(fileDTO));
               
                var filesDataEntity = new FilesDataEntity();
                filesDataEntity.SetId(filesDataRequest.Id);
                filesDataEntity.IdentityLocalName = filesDataRequest.IdentityName;
                filesDataEntity.SetFileDataEntities(filesDataAccessToConvert);

                return filesDataEntity;
            }
            return null;
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private FileDataEntity ConvertToFileDataAccess(FileDataRequestClient fileDataRequest)
        {
            return new FileDataEntity()
            {
                ColorPrint = fileDataRequest.ColorPrint,
                FilePath = fileDataRequest.FilePath,
                FileDataSourceClient = fileDataRequest?.FileDataSource,
            };
        }

    }
}
