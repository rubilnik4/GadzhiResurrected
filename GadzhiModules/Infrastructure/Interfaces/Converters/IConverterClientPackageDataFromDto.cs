﻿using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Threading.Tasks;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;

namespace GadzhiModules.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертеры из трансферной модели в локальную
    /// </summary>  
    public interface IConverterClientPackageDataFromDto
    {
        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        PackageStatus ToPackageStatusFromIntermediateResponse(PackageDataShortResponseClient packageDataShortResponse);

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        PackageStatus ToPackageStatus(PackageDataResponseClient packageDataResponse);


        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        Task<PackageStatus> ToFilesStatusAndSaveFiles(PackageDataResponseClient packageDataResponse);

    }
}
