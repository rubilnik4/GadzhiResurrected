using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOServer.Contracts.Signatures
{
    [ServiceContract]
    public interface ITestService
    {
        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>
        [OperationContract]
        Task<string> GetTest();
    }
}