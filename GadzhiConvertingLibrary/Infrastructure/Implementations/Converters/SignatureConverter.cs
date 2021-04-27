using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
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
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public SignatureConverter(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Сохранить изображения подписей асинхронно
        /// </summary>
        public async Task<IReadOnlyList<ISignatureFile>> ToSignaturesFileAsync(IEnumerable<ISignatureFileData> signaturesFileData,
                                                                               string signatureFolder)
        {
            var signatureTasks = signaturesFileData.Select(signatureFileData => ToSignatureFileAsync(signatureFileData, signatureFolder));
            var signatures = await Task.WhenAll(signatureTasks);

            return signatures.
                   Where(successAndSignature => successAndSignature.success).
                   Select(successAndSignature => successAndSignature.signatureFile).
                   ToList();
        }

        /// <summary>
        /// Сохранить изображения подписей
        /// </summary>
        public IReadOnlyList<ISignatureFile> ToSignaturesFile(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder) =>
            signaturesFileData.
            Select(signatureDto => ToSignatureFile(signatureDto, signatureFolder)).
            Where(successAndSignature => successAndSignature.success).
            Select(successAndSignature => successAndSignature.signatureFile).
            ToList();

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
        private async Task<(bool success, ISignatureFile signatureFile)> ToSignatureFileAsync(ISignatureFileData signatureFileData,
                                                                                              string signatureFolder)
        {
            bool success = await _fileSystemOperations.SaveFileFromByte(CombineFilePath(signatureFolder, signatureFileData.PersonId,
                                                                                        SignatureFile.SaveFormat),
                                                                        signatureFileData.SignatureFileDataSource);

            return (success, new SignatureFile(signatureFileData.PersonId, signatureFileData.PersonInformation,
                                               signatureFolder, signatureFileData.IsVerticalImage));
        }

        /// <summary>
        /// Сохранить изображения подписи
        /// </summary>
        private (bool success, ISignatureFile signatureFile) ToSignatureFile(ISignatureFileData signatureFileData, string signatureFolder)
        {
            string signatureFilePath = SignatureFile.GetFilePathByFolder(signatureFolder, signatureFileData.PersonId,
                                                                         signatureFileData.IsVerticalImage);
            bool success = _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureFileData.SignatureFileDataSource).
                                                 WaitAndUnwrapException();

            return (success, new SignatureFile(signatureFileData.PersonId, signatureFileData.PersonInformation,
                                               signatureFilePath, signatureFileData.IsVerticalImage));
        }

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