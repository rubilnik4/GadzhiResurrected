using GadzhiDTOClient.TransferModels.FilesConvert;
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
        /// <summary>
        /// Идентефикатор пакета
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Идентефикатор пользователя
        /// </summary>
        public string IdentityName { get; }

        /// <summary>
        /// Закрыта ли сессия
        /// </summary>
        public bool IsClosed { get; set; }

        public Authentication()
        {
            IdentityName = GetIdentityName();
            IsClosed = false;
        }

        /// <summary>
        /// Добавить данные о пользователе для запроса на конвертацию
        /// </summary>        
        public FilesDataRequestClient AuthenticateFilesData(FilesDataRequestClient filesDataRequest)
        {
            if (filesDataRequest != null)
            {
                Id = filesDataRequest.Id;
                filesDataRequest.IdentityName = IdentityName;
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