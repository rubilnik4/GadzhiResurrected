using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters;
using Nito.AsyncEx.Synchronous;
using static GadzhiCommon.Infrastructure.Implementations.FileSystemOperations;

namespace GadzhiConvertingLibrary.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование типов подписей
    /// </summary>
    public class SignatureConverter : ISignatureConverter
    {
        public SignatureConverter(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Сохранить изображения подписей асинхронно
        /// </summary>
        public async Task<IResultCollection<ISignatureFile>> ToSignaturesFileAsync(IEnumerable<ISignatureFileData> signaturesFileData,
                                                                               string signatureFolder)
        {
            var signatureTasks = signaturesFileData.Select(signatureFileData => ToSignatureFileAsync(signatureFileData, signatureFolder));
            var signatures = await Task.WhenAll(signatureTasks);
            return signatures.ToResultCollection();
        }

        /// <summary>
        /// Сохранить изображения подписей
        /// </summary>
        public IResultCollection<ISignatureFile> ToSignaturesFile(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder) =>
            signaturesFileData.
            Select(signatureDto => ToSignatureFile(signatureDto, signatureFolder)).
            ToResultCollection();

        /// <summary>
        /// Получить изображения подписей асинхронно
        /// </summary>
        public async Task<IReadOnlyList<ISignatureFileData>> FromSignaturesFileAsync(IEnumerable<ISignatureFile> signaturesFile)
        {
            var signatureTasks = signaturesFile.Select(FromSignatureFileAsync);
            var signatures = await Task.WhenAll(signatureTasks);
            return signatures.ToList();
        }

        /// <summary>
        /// Получить изображения подписей асинхронно
        /// </summary>
        public IReadOnlyList<ISignatureFileData> FromSignaturesFile(IEnumerable<ISignatureFile> signaturesFile) =>
            signaturesFile.Select(FromSignatureFile).ToList();

        /// <summary>
        /// Сохранить изображения подписи асинхронно
        /// </summary>
        private async Task<IResultValue<ISignatureFile>> ToSignatureFileAsync(ISignatureFileData signatureFileData,
                                                                              string signatureFolder) =>
             await _fileSystemOperations.SaveFileFromByte(FilePathOperations.CombineFilePath(signatureFolder, signatureFileData.PersonId, SignatureFile.SaveFormat),
                                                          signatureFileData.SignatureFileDataSource).
             ResultValueOkAsync(filePath => new SignatureFile(signatureFileData.PersonId, signatureFileData.PersonInformation,
                                                              signatureFolder, signatureFileData.IsVerticalImage));

        /// <summary>
        /// Сохранить изображения подписи
        /// </summary>
        private IResultValue<ISignatureFile> ToSignatureFile(ISignatureFileData signatureFileData, string signatureFolder) =>
            SignatureFile.GetFilePathByFolder(signatureFolder, signatureFileData.PersonId, signatureFileData.IsVerticalImage).
            Map(signatureFilePath => _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureFileData.SignatureFileDataSource).
                                     WaitAndUnwrapException()).
            ResultValueOk(signatureFilePath => new SignatureFile(signatureFileData.PersonId, signatureFileData.PersonInformation,
                                                                 signatureFilePath, signatureFileData.IsVerticalImage));

        /// <summary>
        /// Получить изображения подписи асинхронно
        /// </summary>
        private async Task<ISignatureFileData> FromSignatureFileAsync(ISignatureFile signatureFile)
        {
            var signatureFileData = await _fileSystemOperations.GetFileFromPath(signatureFile.SignatureFilePath);
            return new SignatureFileData(signatureFile.PersonId, signatureFile.PersonInformation,
                                         signatureFileData, signatureFile.IsVerticalImage);
        }

        /// <summary>
        /// Получить изображения подписи
        /// </summary>
        private ISignatureFileData FromSignatureFile(ISignatureFile signatureFile)
        {
            var signatureFileData = _fileSystemOperations.GetFileFromPath(signatureFile.SignatureFilePath).
                                                          WaitAndUnwrapException();
            return new SignatureFileData(signatureFile.PersonId, signatureFile.PersonInformation,
                                         signatureFileData, signatureFile.IsVerticalImage);
        }
    }
}