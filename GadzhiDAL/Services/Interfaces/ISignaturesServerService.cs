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
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        Task<IList<SignatureDto>> GetSignatures(IList<string> ids);

        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        Task UploadSignatures(IList<SignatureDto> signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>      
        Task<SignatureMicrostationDto> GetSignaturesMicrostation();

        /// <summary>
        /// Записать подписи Microstation в базу данных
        /// </summary>      
        Task UploadSignaturesMicrostation(SignatureMicrostationDto signatureMicrostationDto);
    }
}