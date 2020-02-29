using GadzhiDAL.Entities.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Entities.FilesConvert.Archive
{
    /// <summary>
    /// Информация об отконвертированных файлах в базе данных
    /// </summary>
    public class FileDataSourceArchiveEntity : FileDataSourceEntityBase
    {
        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FileDataArchiveEntity FileDataArchiveEntity { get; set; }
    }
}
