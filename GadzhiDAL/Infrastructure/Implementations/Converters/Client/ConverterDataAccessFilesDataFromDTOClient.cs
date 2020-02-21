using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Linq;

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
            var filesDataAccessToConvert = filesDataRequest?.FilesData?.Select(fileDTO =>
                                               ConvertToFileDataAccess(fileDTO));

            var filesDataEntity = new FilesDataEntity();
            filesDataEntity.SetId(filesDataRequest.Id);
            filesDataEntity.IdentityMachine.IdentityLocalName = filesDataRequest.IdentityName;
            filesDataEntity.SetFilesData(filesDataAccessToConvert);

            return filesDataEntity;
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
                FileDataSource = fileDataRequest.FileDataSource,
            };
        }

    }
}
