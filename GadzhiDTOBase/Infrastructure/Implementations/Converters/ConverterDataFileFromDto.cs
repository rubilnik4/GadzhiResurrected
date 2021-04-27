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
using Nito.AsyncEx.Synchronous;

namespace GadzhiDTOBase.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public class ConverterDataFileFromDto : IConverterDataFileFromDto
    {
        public ConverterDataFileFromDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Преобразовать подписи из трансферной модели
        /// </summary>
        public static IReadOnlyList<ISignatureFileData> SignaturesFileDataFromDto(IEnumerable<SignatureDto> signaturesDto) =>
            signaturesDto.
            Select(SignatureFileDataFromDto).
            ToList();

        /// <summary>
        /// Преобразовать подписи из трансферной модели
        /// </summary>
        public static IReadOnlyList<ISignatureLibrary> SignaturesLibraryFromDto(IEnumerable<SignatureDto> signaturesDto) =>
            signaturesDto.
            Select(SignatureLibraryFromDto).ToList();

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения
        /// </summary>
        public IReadOnlyList<ISignatureFile> SignaturesFileFromDto(IEnumerable<SignatureDto> signaturesDto, string signatureFolder) =>
            signaturesDto.
            Select(signatureDto => SignatureFileFromDto(signatureDto, signatureFolder)).
            Where(successAndSignature => successAndSignature.success).
            Select(successAndSignature => successAndSignature.signatureFile).
            ToList();

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения асинхронно
        /// </summary>
        public async Task<IReadOnlyList<ISignatureFile>> SignaturesFileFromDtoAsync(IEnumerable<SignatureDto> signaturesDto, string signatureFolder)
        {
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
        private static ISignatureFileData SignatureFileDataFromDto(SignatureDto signatureDto) =>
            new SignatureFileData(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation),
                                  signatureDto.SignatureJpeg, false);

        /// <summary>
        /// Преобразовать подпись из трансферной модели
        /// </summary>
        private static ISignatureLibrary SignatureLibraryFromDto(SignatureDto signatureDto) =>
            new SignatureLibrary(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation));

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи
        /// </summary>
        private (bool success, ISignatureFile signatureFile) SignatureFileFromDto(SignatureDto signatureDto, string signatureFolder)
        {
            if (signatureDto == null) throw new ArgumentNullException(nameof(signatureDto));
            string signatureFilePath = SignatureFile.GetFilePathByFolder(signatureFolder, signatureDto.PersonId, false);
            bool success = _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureDto.SignatureJpeg).WaitAndUnwrapException();

            return (success, new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation), 
                                               signatureFilePath, false));
        }

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи асинхронно
        /// </summary>
        private async Task<(bool success, ISignatureFile signatureFile)> SignatureFileFromDtoAsync(SignatureDto signatureDto, string signatureFolder)
        {
            string signatureFilePath = SignatureFile.GetFilePathByFolder(signatureFolder, signatureDto.PersonId, false);
            bool success = await _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureDto.SignatureJpeg);

            return (success, new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation),
                                               signatureFilePath, false));
        }

        /// <summary>
        /// Преобразовать информацию о пользователе из трансферной модели
        /// </summary>
        private static PersonInformation PersonInformationFromDto(PersonInformationDto personInformation) =>
            (personInformation != null)
                ? new PersonInformation(personInformation.Surname, personInformation.Name, personInformation.Patronymic, personInformation.DepartmentType)
                : throw new ArgumentNullException(nameof(personInformation));
    }
}