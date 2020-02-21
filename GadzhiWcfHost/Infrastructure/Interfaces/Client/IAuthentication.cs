using GadzhiDTOClient.TransferModels.FilesConvert;
using System;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
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