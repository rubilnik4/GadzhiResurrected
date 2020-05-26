using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiDTOBase.Infrastructure.Interfaces.Converters;
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
        public static Func<IEnumerable<string>, IList<ISignatureFileApp>> GetSignaturesSync(IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                                                                            IConverterDataFileFromDto converterDataFileFromDto,
                                                                                            string signatureFolder) => 
            (idSignatures) => fileConvertingServerService.Operations.GetSignatures(idSignatures.ToList()).WaitAndUnwrapException().
                              Map(signatures => converterDataFileFromDto.SignaturesFileFromDto(signatures, signatureFolder)).
                              ToApplication().ToList();
    }
}
