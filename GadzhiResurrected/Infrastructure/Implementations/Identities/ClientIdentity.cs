using System;

namespace GadzhiResurrected.Infrastructure.Implementations.Identities
{
    /// <summary>
    /// Имя пользователя при авторизации Windows
    /// </summary>
    public static class ClientIdentity
    {
        /// <summary>
        /// Имя пользователя при авторизации Windows
        /// </summary>
        public static string ClientIdentityName =>
            Environment.UserDomainName + "\\" + Environment.UserName;
    }
}