using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Идентефикация пользователя
    /// </summary>
    public class Authentication: IAuthentication
    {
        public Authentication()
        {

        }

        /// <summary>
        /// Добавить данные о пользователе для запроса на конвертацию
        /// </summary>        
        public FilesDataRequest AuthenticateFilesData(FilesDataRequest filesDataRequest)
        {
            if (filesDataRequest != null)
            {
                filesDataRequest.IdentityName = GetIdentityName();
            }

            return filesDataRequest;
        }           

        /// <summary>
        /// Получить имя пользователя и домен
        /// </summary>      
        private string GetIdentityName() => OperationContext.Current.
                   ServiceSecurityContext.PrimaryIdentity.Name;       
    }
}