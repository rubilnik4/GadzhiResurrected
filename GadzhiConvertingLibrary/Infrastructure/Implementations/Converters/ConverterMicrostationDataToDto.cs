using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiConvertingLibrary.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public static class ConverterMicrostationDataToDto
    {
        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFileDto MicrostationDataFileToDto(MicrostationDataFile microstationDataFile) =>
            new MicrostationDataFileDto(microstationDataFile.NameDatabase, microstationDataFile.MicrostationDataBase);
    }
}