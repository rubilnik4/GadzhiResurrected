using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах
    /// </summary>
    public interface IFilesDataService
    {
        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary> 
        Task AddFilesDataAsync(FilesDataEntity filesDataEntity);      
    }
}
