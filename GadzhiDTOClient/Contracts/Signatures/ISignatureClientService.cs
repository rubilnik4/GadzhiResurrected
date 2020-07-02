using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOClient.Contracts.Signatures
{
    /// <summary>
    /// Сервис для получения информации о подписях. Контракт используется клиентской частью
    /// </summary>
    [ServiceContract]
    public interface ISignatureClientService
    {
        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>
        [OperationContract]
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        [OperationContract]
        Task<IList<DepartmentType>> GetSignaturesDepartments();
    }
}