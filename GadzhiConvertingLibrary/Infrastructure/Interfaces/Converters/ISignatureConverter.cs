using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters
{
    public interface ISignatureConverter
    {
        /// <summary>
        /// Сохранить изображения подписей асинхронно
        /// </summary>
        Task<IResultCollection<ISignatureFile>> ToSignaturesFileAsync(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder);

        /// <summary>
        /// Сохранить изображения подписей
        /// </summary>
        IResultCollection<ISignatureFile> ToSignaturesFile(IEnumerable<ISignatureFileData> signaturesFileData, string signatureFolder);

        /// <summary>
        /// Получить изображения подписей асинхронно
        /// </summary>
        Task<IReadOnlyList<ISignatureFileData>> FromSignaturesFileAsync(IEnumerable<ISignatureFile> signaturesFileData);

        /// <summary>
        /// Получить изображения подписей
        /// </summary>
        IReadOnlyList<ISignatureFileData> FromSignaturesFile(IEnumerable<ISignatureFile> signaturesFileData);
    }
}