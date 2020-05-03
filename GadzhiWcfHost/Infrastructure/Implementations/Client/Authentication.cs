using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.ServiceModel;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
{
    /// <summary>
    /// Идентификация пользователя
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// Получить имя пользователя и домен
        /// </summary>      
        public static string GetIdentityName() => OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
    }
}