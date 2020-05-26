using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOBase.Infrastructure.Implementations.Converters
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
        public IReadOnlyList<ISignatureFile> SignaturesFileFromDto(IList<SignatureDto> signaturesDto, string signatureFolder) =>
            signaturesDto?.
            Select(signatureDto => SignatureFileFromDto(signatureDto, signatureFolder)).
            Where(successAndSignature => successAndSignature.success).
            Select(successAndSignature => successAndSignature.signatureFile).
            ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения асинхронно
        /// </summary>
        public async Task<IReadOnlyList<ISignatureFile>> SignaturesFileFromDtoAsync(IReadOnlyList<SignatureDto> signaturesDto, string signatureFolder)
        {
            if (signaturesDto == null) throw new ArgumentNullException(nameof(signaturesDto));

            var signatureTasks = signaturesDto.Select(signatureDto => SignatureFileFromDtoAsync(signatureDto, signatureFolder));
            var signatures = await Task.WhenAll(signatureTasks);

            return signatures.
                   Where(successAndSignature => successAndSignature.success).
                   Select(successAndSignature => successAndSignature.signatureFile).
                   ToList();
        }

        /// <summary>
        /// Преобразовать подпись из трансферной модели
        /// </summary>
        private static ISignatureLibrary SignatureLibraryFromDto(SignatureDto signatureDto) =>
            (signatureDto != null)
                ? new SignatureLibrary(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation))
                : throw new ArgumentNullException(nameof(signatureDto));

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи
        /// </summary>
        private (bool success, ISignatureFile signatureFile) SignatureFileFromDto(SignatureDto signatureDto, string signatureFolder)
        {
            if (signatureDto == null) throw new ArgumentNullException(nameof(signatureDto));
            bool success = _fileSystemOperations.SaveFileFromByte(FileSystemOperations.CombineFilePath(signatureFolder, signatureDto.PersonId, SignatureFile.SaveFormat),
                                                                  signatureDto.SignatureJpeg).Result;

            return (success, new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation), signatureFolder));
        }

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи асинхронно
        /// </summary>
        private async Task<(bool success, ISignatureFile signatureFile)> SignatureFileFromDtoAsync(SignatureDto signatureDto, string signatureFolder)
        {
            if (signatureDto == null) throw new ArgumentNullException(nameof(signatureDto));
            bool success = await _fileSystemOperations.SaveFileFromByte(FileSystemOperations.CombineFilePath(signatureFolder, signatureDto.PersonId,
                                                                                                             SignatureFile.SaveFormat),
                                                                        signatureDto.SignatureJpeg);

            return (success, new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation), signatureFolder));
        }

        /// <summary>
        /// Преобразовать информацию о пользователе из трансферной модели
        /// </summary>
        private static PersonInformation PersonInformationFromDto(PersonInformationDto personInformation) =>
            (personInformation != null)
                ? new PersonInformation(personInformation.Surname, personInformation.Name, personInformation.Patronymic, personInformation.Department)
                : throw new ArgumentNullException(nameof(personInformation));
    }
}