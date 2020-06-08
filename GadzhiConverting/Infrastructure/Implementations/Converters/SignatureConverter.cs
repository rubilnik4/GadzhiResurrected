using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using Nito.AsyncEx.Synchronous;
using static GadzhiCommon.Infrastructure.Implementations.FileSystemOperations;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование типов подписей
    /// </summary>
    public class SignatureConverter: ISignatureConverter
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public SignatureConverter(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Сохранить изображения подписей асинхронно
        /// </summary>
        public async Task<IReadOnlyList<ISignatureFile>> ToSignaturesFileAsync(IEnumerable<ISignatureFileData> signaturesFileData, 
                                                                               string signatureFolder)
        {
            if (signaturesFileData == null) throw new ArgumentNullException(nameof(signaturesFileData));

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
        public IReadOnlyList<ISignatureFile> ToSignaturesFile(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder)=>
            signaturesFileData?.
            Select(signatureDto => ToSignatureFile(signatureDto, signatureFolder)).
            Where(successAndSignature => successAndSignature.success).
            Select(successAndSignature => successAndSignature.signatureFile).
            ToList()
            ?? throw new ArgumentNullException(nameof(signaturesFileData));

        /// <summary>
        /// Сохранить изображения подписи асинхронно
        /// </summary>
        private async Task<(bool success, ISignatureFile signatureFile)> ToSignatureFileAsync(ISignatureFileData signatureFileData, 
                                                                                              string signatureFolder)
        {
            if (signatureFileData == null) throw new ArgumentNullException(nameof(signatureFileData));
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
            if (signatureFileData == null) throw new ArgumentNullException(nameof(signatureFileData));
            string signatureFilePath = SignatureFile.GetFilePathByFolder(signatureFolder, signatureFileData.PersonId, 
                                                                         signatureFileData.IsVerticalImage);
            bool success = _fileSystemOperations.SaveFileFromByte(signatureFilePath, signatureFileData.SignatureFileDataSource).
                                                 WaitAndUnwrapException();

            return (success, new SignatureFile(signatureFileData.PersonId, signatureFileData.PersonInformation, 
                                               signatureFilePath, signatureFileData.IsVerticalImage));
        }


    }
}