using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiWcfHost.Infrastructure.Implementations.Client;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;

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
        /// <summary>
        /// Сохранение, обработка, подготовка для отправки файлов
        /// </summary>   
        private readonly IApplicationClientConverting _applicationClientConverting;

        public SignatureClientService(IApplicationClientConverting applicationClientConverting)
        {
            _applicationClientConverting = applicationClientConverting;
            OperationContext.Current.InstanceContext.Closed += InstanceContext_Closed;
        }

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => await _applicationClientConverting.GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() => await _applicationClientConverting.GetSignaturesDepartments();

        private static void InstanceContext_Closed(object sender, EventArgs e)
        {
            // Session closed here
        }
    }
}
