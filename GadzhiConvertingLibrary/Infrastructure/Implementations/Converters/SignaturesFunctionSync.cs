using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Implementations.Images;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConvertingLibrary.Extensions;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using Nito.AsyncEx.Synchronous;

namespace GadzhiConvertingLibrary.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование функций получения подписей в синхронный вариант
    /// </summary>
    public static class SignaturesFunctionSync
    {
        /// <summary>
        /// Получить подписи по идентификаторам синхронно 
        /// </summary>
        [Logger]
        public static Func<IList<SignatureFileRequest>, IResultAppCollection<ISignatureFileApp>> GetSignaturesSync(SignatureServerServiceFactory signatureServerServiceFactory,
                                                                                                                   ISignatureConverter signatureConverter,
                                                                                                                   string signatureFolder) =>
            signatureFileRequest => GetSignaturesSyncList(signatureServerServiceFactory, signatureConverter, signatureFolder).
                                    Map(getSignaturesFunc => getSignaturesFunc(signatureFileRequest.ToList()));

        /// <summary>
        /// Получить подписи по идентификаторам синхронно из папки 
        /// </summary>
        [Logger]
        public static Func<IList<SignatureFileRequest>, IResultAppCollection<ISignatureFileApp>> GetSignaturesSync(IResultCollection<ISignatureFile> signatureFile,
                                                                                                                   ISignatureConverter signatureConverter, string signatureFolder) =>
            signaturesFileRequest =>
                signaturesFileRequest.
                Select(signatureRequest => signatureRequest.PersonId).
                Map(ids => signatureFile.ResultValueOk(signatures => signatures.Where(signature => ids.Contains(signature.PersonId)))).
                ResultValueOk(signatureConverter.FromSignaturesFile).
                ToResultValueApplication().
                Map(signatures => GetSignaturesFileApp(signatures, signaturesFileRequest, signatureConverter, signatureFolder));

        /// <summary>
        /// Получить подписи по идентификаторам синхронно 
        /// </summary>

        private static Func<IList<SignatureFileRequest>, IResultAppCollection<ISignatureFileApp>> GetSignaturesSyncList(SignatureServerServiceFactory signatureServerServiceFactory,
                                                                                                                        ISignatureConverter signatureConverter, string signatureFolder) =>
            signaturesFileRequest =>
                signaturesFileRequest.
                    Select(signatureRequest => signatureRequest.PersonId).
                    Map(ids => signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.GetSignatures(ids.ToList()))).
                    WaitAndUnwrapException().
                    ToResultValueApplication().
                    ResultValueOk(ConverterDataFileFromDto.SignaturesFileDataFromDto).
                    Map(signatures => GetSignaturesFileApp(signatures, signaturesFileRequest, signatureConverter, signatureFolder));

        /// <summary>
        /// Получить сохраненные подписи
        /// </summary>
        private static IResultAppCollection<ISignatureFileApp> GetSignaturesFileApp(IResultAppValue<IReadOnlyCollection<ISignatureFileData>> signatures,
                                                                                    IList<SignatureFileRequest> signaturesFileRequest,
                                                                                    ISignatureConverter signatureConverter, string signatureFolder) =>
            signatures.
            ResultValueOk(signaturesFileData => signaturesFileRequest.
                              Join(signaturesFileData,
                                   signatureFileRequest => signatureFileRequest.PersonId,
                                   signatureFileData => signatureFileData.PersonId,
                                   (signatureFileRequest, signatureFileData) => RotateSignature(signatureFileData, signatureFileRequest))).
            ResultValueOkBind(signaturesFileData => signatureConverter.ToSignaturesFile(signaturesFileData, signatureFolder).
                                                    ResultValueOk(signatureFiles => signatureFiles.ToApplication()).
                                                    ToResultValueApplication()).
            ToResultCollection();

        /// <summary>
        /// Повернуть изображение подписи
        /// </summary>
        private static ISignatureFileData RotateSignature(ISignatureFileData signatureFileData, SignatureFileRequest signatureFileRequest) =>

            signatureFileRequest.IsVerticalImage
                ? new SignatureFileData(signatureFileData.PersonId, signatureFileData.PersonInformation,
                                        ImageOperations.RotateImageInByte(signatureFileData.SignatureFileDataSource,
                                                                          ImageRotation.Rotate270, ImageFormatApplication.Jpeg),
                                        true)
                : signatureFileData;
    }
}
