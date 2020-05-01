using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.ServiceModel;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
{
    /// <summary>
    /// Идентефикация пользователя
    /// </summary>
    public class Authentication : IAuthentication
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
        public PackageDataRequestClient AuthenticateFilesData(PackageDataRequestClient packageDataRequest)
        {
            if (packageDataRequest != null)
            {
                Id = packageDataRequest.Id;
                packageDataRequest.IdentityName = IdentityName;
            }

            return packageDataRequest;
        }

        /// <summary>
        /// Получить имя пользователя и домен
        /// </summary>      
        private string GetIdentityName() => OperationContext.Current.
                   ServiceSecurityContext.PrimaryIdentity.Name;
    }
}