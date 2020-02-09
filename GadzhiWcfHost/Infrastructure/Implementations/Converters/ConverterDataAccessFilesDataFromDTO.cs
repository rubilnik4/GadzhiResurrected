using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Interfaces.Converters;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Implementations.Converters
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
            var filesDataEntity = new FilesDataEntity()
            {
                IdGuid = filesDataRequest.ID,
                IsCompleted = false,
                StatusProcessingProject = StatusProcessingProject.InQueue
            };
            filesDataEntity.AddRangeFilesData(filesDataAccessToConvert);
            return filesDataEntity;
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private FileDataEntity ConvertToFileDataAccess(FileDataRequest fileDataRequest)
        {
            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid(fileDataRequest);

            return new FileDataEntity()
            {
                IsCompleted = false,
                ColorPrint = fileDataRequest.ColorPrint,
                FilePath = fileDataRequest.FilePath,
                StatusProcessing = StatusProcessing.InQueue,
                FileDataSource = fileDataRequest.FileDataSource,
                FileConvertErrorType = errorsFromValidation.ToList(),
            };
        }
    }
}
