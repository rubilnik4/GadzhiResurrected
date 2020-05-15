using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiDAL.Services.Interfaces
{
    /// <summary>
    /// Получение и запись из БД подписей и идентификаторов
    /// </summary>
    public interface ISignaturesServerService
    {
        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        Task UploadSignatures(IList<SignatureDto> signaturesDto);
    }
}