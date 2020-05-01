using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System.Threading.Tasks;

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
        PackageStatus ToPackageStatusFromIntermediateResponse(PackageDataIntermediateResponseClient packageDataIntermediateResponse);

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
