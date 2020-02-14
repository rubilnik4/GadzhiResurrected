using GadzhiDTO.TransferModels.FilesConvert;
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
        /// Добавить данные о пользователе для запроса на конвертацию
        /// </summary>        
        FilesDataRequest AuthenticateFilesData(FilesDataRequest filesDataRequest);
    }
}