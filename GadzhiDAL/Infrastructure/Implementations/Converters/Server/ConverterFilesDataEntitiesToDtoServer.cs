﻿using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Components;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public static class ConverterFilesDataEntitiesToDtoServer
    {
        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public static PackageDataRequestServer PackageDataToRequest(PackageDataEntity packageDataEntity) =>
            packageDataEntity != null
                ? new PackageDataRequestServer(Guid.Parse(packageDataEntity.Id),
                                               ConvertingSettingsToRequest(packageDataEntity.ConvertingSettings),
                                               packageDataEntity.FileDataEntities.Select(FileDataToRequest).ToList(),
                                               packageDataEntity.AttemptingConvertCount)
                : PackageDataRequestServer.EmptyPackage;

        /// <summary>
        /// Преобразовать параметры конвертации в трансферную модель
        /// </summary>
        private static ConvertingSettingsRequest ConvertingSettingsToRequest(ConvertingSettingsComponent convertingSettings) =>
            new ConvertingSettingsRequest(convertingSettings.PersonId, convertingSettings.PdfNamingType,
                                           convertingSettings.ConvertingModeTypesList.ToList(), 
                                           convertingSettings.UseDefaultSignature);

        /// <summary>
        /// Конвертировать файл модели базы данных в запрос
        /// </summary>
        private static FileDataRequestServer FileDataToRequest(FileDataEntity fileDataEntity) =>
            new FileDataRequestServer(fileDataEntity.FilePath, fileDataEntity.ColorPrintType, fileDataEntity.StatusProcessing, 
                                      fileDataEntity.FileDataSource.ToArray(), fileDataEntity.FileExtensionAdditional,
                                      fileDataEntity.FileDataSourceAdditional?.ToArray());
        
    }
}
