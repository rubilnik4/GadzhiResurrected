using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces;

namespace GadzhiConverting.Infrastructure.Implementations.Converting
{
    /// <summary>
    /// Проверка перед стартом
    /// </summary>
    public static class ConvertingValidation
    {
        /// <summary>
        /// Проверить наличие базы подписей
        /// </summary>
        public static bool CheckSignatures(IProjectSettings projectSettings, IMessagingService messagingService) =>
            projectSettings.ConvertingResources.SignatureNames.
            Map(signatures => signatures.OkStatus && signatures.Value.Count > 0).
            WhereBad(hasSignatures => hasSignatures,
                badFunc: hasSignatures => hasSignatures.
                         Void(_ => messagingService.ShowAndLogError(new ErrorCommon(ErrorConvertingType.SignatureNotFound,
                                                                                     "База подписей не загружена. Отмена запуска"))));

        /// <summary>
        /// Проверить наличие принтеров
        /// </summary>
        public static bool CheckPrinters(IProjectSettings projectSettings, IMessagingService messagingService) =>
            projectSettings.PrintersInformation.
            Map(printersInformation => printersInformation.OkStatus).
            WhereBad(hasPrinters => hasPrinters,
                badFunc: hasPrinters => hasPrinters.
                         Void(_ => messagingService.ShowAndLogError(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                                     "Принтеры не установлены"))));
    }
}