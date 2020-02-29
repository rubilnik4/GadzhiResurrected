using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Archive
{
    /// <summary>
    /// Конвертер в архивную версию
    /// </summary>    
    public interface IConverterToArchive
    {
        /// <summary>
        /// Конвертировать пакет в архивную версию базы данных
        /// </summary>    
        Task<FilesDataArchiveEntity> ConvertFilesDataToArchive(FilesDataEntity filesDataEntity);
    }
}
