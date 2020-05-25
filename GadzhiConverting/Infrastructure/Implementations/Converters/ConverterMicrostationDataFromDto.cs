using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public static class ConverterMicrostationDataFromDto
    {
        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFile MicrostationDataFileFromDto(MicrostationDataFileDto microstationDataFileDto) =>
            (microstationDataFileDto != null)
            ? new MicrostationDataFile(microstationDataFileDto.NameDatabase, microstationDataFileDto.MicrostationDataBase)
            : throw new ArgumentNullException(nameof(microstationDataFileDto));
    }
}