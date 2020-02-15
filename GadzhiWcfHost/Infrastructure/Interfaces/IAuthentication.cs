using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Идентефикация пользователя
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Идентефикатор пакета
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Идентефикатор пользователя
        /// </summary>
        string IdentityName { get; }

        /// <summary>
        /// Закрыта ли сессия
        /// </summary>
        bool IsClosed { get; set; }

        /// <summary>
        /// Добавить данные о пользователе для запроса на конвертацию
        /// </summary>        
        FilesDataRequestClient AuthenticateFilesData(FilesDataRequestClient filesDataRequest);
    }
}