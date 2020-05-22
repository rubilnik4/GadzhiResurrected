using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiDAL.Services.Interfaces
{
    /// <summary>
    /// Получение и запись из БД подписей и идентификаторов
    /// </summary>
    public interface ISignaturesService
    {
        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>      
        Task<IList<string>> GetSignaturesDepartments();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        Task<IList<SignatureDto>> GetSignatures(IList<string> ids);

        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        Task UploadSignatures(IList<SignatureDto> signaturesDto);

        /// <summary>
        /// Получить данные Microstation из базы данных
        /// </summary>      
        Task<MicrostationDataFileDto> GetMicrostationDataFile(string idDataFile);

        /// <summary>
        /// Записать данные Microstation в базу данных
        /// </summary>      
        Task UploadMicrostationDataFile(MicrostationDataFileDto microstationDataFileDto, string idDataFile);
    }
}