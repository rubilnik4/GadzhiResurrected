using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters
{
    public interface ISignatureConverter
    {
        /// <summary>
        /// Сохранить изображения подписей асинхронно
        /// </summary>
        Task<IReadOnlyList<ISignatureFile>> ToSignaturesFileAsync(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder);

        /// <summary>
        /// Сохранить изображения подписей
        /// </summary>
        IReadOnlyList<ISignatureFile> ToSignaturesFile(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder);
    }
}