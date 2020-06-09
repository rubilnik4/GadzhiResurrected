using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Images;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using Nito.AsyncEx.Synchronous;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование функций получения подписей в синхронный вариант
    /// </summary>
    public static class SignaturesFunctionSync
    {
        /// <summary>
        /// Получить подписи по идентификаторам синхронно 
        /// </summary>
        public static Func<IEnumerable<SignatureFileRequest>, IList<ISignatureFileApp>> GetSignaturesSync(IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                                                                                          ISignatureConverter signatureConverter,
                                                                                                          string signatureFolder) =>
            (signatureFileRequest) => GetSignaturesSyncList(fileConvertingServerService, signatureConverter, signatureFolder).
                                      Map(getSignaturesFunc => getSignaturesFunc(signatureFileRequest.ToList()));

        /// <summary>
        /// Получить подписи по идентификаторам синхронно 
        /// </summary>
        private static Func<IList<SignatureFileRequest>, IList<ISignatureFileApp>> GetSignaturesSyncList(IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                                                                                            ISignatureConverter signatureConverter,
                                                                                                            string signatureFolder) =>
            (signaturesFileRequest) =>
                signaturesFileRequest?.
                Select(signatureRequest => signatureRequest.PersonId).
                Map(ids => fileConvertingServerService.Operations.GetSignatures(ids.ToList()).WaitAndUnwrapException()).
                Map(ConverterDataFileFromDto.SignaturesFileDataFromDto).
                Map(signaturesFileData => signaturesFileRequest.Join(signaturesFileData,
                                                                     signatureFileRequest => signatureFileRequest.PersonId,
                                                                     signatureFileData => signatureFileData.PersonId,
                                                                     (signatureFileRequest, signatureFileData) => RotateSignature(signatureFileData, 
                                                                                                                                  signatureFileRequest))).
                Map(signaturesFileData => signatureConverter.ToSignaturesFile(signaturesFileData, signatureFolder)).
                ToApplication().ToList();

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
