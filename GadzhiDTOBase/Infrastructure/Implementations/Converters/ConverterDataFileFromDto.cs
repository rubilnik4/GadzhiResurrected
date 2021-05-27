using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.Errors;
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
        public IResultCollection<ISignatureFile> SignaturesFileFromDto(IEnumerable<SignatureDto> signaturesDto, string signatureFolder) =>
            signaturesDto.
            Select(signatureDto => SignatureFileFromDto(signatureDto, signatureFolder)).
            ToResultCollection();

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения асинхронно
        /// </summary>
        public async Task<IResultCollection<ISignatureFile>> SignaturesFileFromDtoAsync(IEnumerable<SignatureDto> signaturesDto, string signatureFolder)
        {
            var signatureTasks = signaturesDto.Select(signatureDto => SignatureFileFromDtoAsync(signatureDto, signatureFolder));
            var signatures = await Task.WhenAll(signatureTasks);
            return signatures.ToResultCollection();
        }

        /// <summary>
        /// Преобразовать подпись из трансферной модели
        /// </summary>
        private static ISignatureFileData SignatureFileDataFromDto(SignatureDto signatureDto) =>
            new SignatureFileData(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation),
                                  signatureDto.SignatureSource, false);

        /// <summary>
        /// Преобразовать подпись из трансферной модели
        /// </summary>
        private static ISignatureLibrary SignatureLibraryFromDto(SignatureDto signatureDto) =>
            new SignatureLibrary(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation));

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи
        /// </summary>
        private IResultValue<ISignatureFile> SignatureFileFromDto(SignatureDto signatureDto, string signatureFolder) =>
            SignatureFile.GetFilePathByFolder(signatureFolder, signatureDto.PersonId, false).
            Map(signatureFilePath => 
                _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureDto.SignatureSourceList).WaitAndUnwrapException().
                ResultValueOk(_ => new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation),
                                                     signatureFilePath, false)));

        /// <summary>
        /// Преобразовать подпись из трансферной модели и сохранить файл подписи асинхронно
        /// </summary>
        private async Task<IResultValue<ISignatureFile>> SignatureFileFromDtoAsync(SignatureDto signatureDto, string signatureFolder) =>
            await SignatureFile.GetFilePathByFolder(signatureFolder, signatureDto.PersonId, false).
            Map(signatureFilePath =>
                _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureDto.SignatureSourceList).
                ResultValueOkAsync(_ => new SignatureFile(signatureDto.PersonId, PersonInformationFromDto(signatureDto.PersonInformation),
                                                          signatureFilePath, false)));

        /// <summary>
        /// Преобразовать информацию о пользователе из трансферной модели
        /// </summary>
        private static PersonInformation PersonInformationFromDto(PersonInformationDto personInformation) => 
            new PersonInformation(personInformation.Surname, personInformation.Name, personInformation.Patronymic,
                                  personInformation.DepartmentType);
    }
}