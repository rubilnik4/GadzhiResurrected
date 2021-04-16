using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOClient.Contracts.Signatures;

namespace GadzhiWcfHost.Services
{
    /// <summary>
    /// Сервис для получения информации о подписях. Контракт используется клиентской частью
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class SignatureClientService : ISignatureClientService
    {
        public SignatureClientService(ISignaturesService signaturesService)
        {
            _signaturesService = signaturesService;
        }

        /// <summary>
        /// Сервис для добавления и получения данных о подписях
        /// </summary>
        private readonly ISignaturesService _signaturesService;

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => 
            await _signaturesService.GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() =>
            await _signaturesService.GetSignaturesDepartments();
    }
}
