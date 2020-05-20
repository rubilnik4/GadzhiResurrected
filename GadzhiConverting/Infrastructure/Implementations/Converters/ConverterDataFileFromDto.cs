using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public class ConverterDataFileFromDto : IConverterDataFileFromDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterDataFileFromDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Преобразовать подписи из трансферной модели
        /// </summary>
        public static IReadOnlyList<ISignatureLibrary> SignaturesLibraryFromDto(IList<SignatureDto> signaturesDto) =>
            signaturesDto?.
            Select(SignatureLibraryFromDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения
        /// </summary>
        public IList<ISignatureFile> SignaturesFileFromDto(IList<SignatureDto> signaturesDto, string signatureFolder) =>
            signaturesDto?.
            Select(signatureDto => SignatureFileFromDto(signatureDto, signatureFolder)).
            Where(successAndSignature => successAndSignature.success).
            Select(successAndSignature => successAndSignature.signatureFile).
            ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFile MicrostationDataFileFromDto(MicrostationDataFileDto microstationDataFileDto) =>
            (microstationDataFileDto != null)
            ? new MicrostationDataFile(microstationDataFileDto.NameDatabase, microstationDataFileDto.MicrostationDataBase)
            : throw new ArgumentNullException(nameof(microstationDataFileDto));

        /// <summary>
        /// Преобразовать подпись из трансферной модели
        /// </summary>
        private static ISignatureLibrary SignatureLibraryFromDto(SignatureDto signatureDto) =>
            (signatureDto != null)
                ? new SignatureLibrary(signatureDto.Id, signatureDto.FullName)
                : throw new ArgumentNullException(nameof(signatureDto));

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи
        /// </summary>
        private (bool success, ISignatureFile signatureFile) SignatureFileFromDto(SignatureDto signatureDto, string signatureFolder)
        {
            if (signatureDto == null) throw new ArgumentNullException(nameof(signatureDto));
            bool success = _fileSystemOperations.SaveFileFromByte(FileSystemOperations.CombineFilePath(signatureFolder, signatureDto.Id, SignatureFile.SaveFormat),
                                                                  signatureDto.SignatureJpeg).Result;
            
            return (success, new SignatureFile(signatureDto.Id, signatureDto.FullName, signatureFolder));
        }
    }
}