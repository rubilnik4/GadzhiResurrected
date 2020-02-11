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
            filesDataEntity.SetFilesData(filesDataAccessToConvert);

            return filesDataEntity;
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private FileDataEntity ConvertToFileDataAccess(FileDataRequest fileDataRequest)
        {
            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid(fileDataRequest);

            var fileDataEntity = new FileDataEntity()
            {
                ColorPrint = fileDataRequest.ColorPrint,
                FilePath = fileDataRequest.FilePath,
                FileDataSource = fileDataRequest.FileDataSource,
            };
            fileDataEntity.SetFileConvertErrorType(errorsFromValidation);

            return fileDataEntity;

        }
    }
}
