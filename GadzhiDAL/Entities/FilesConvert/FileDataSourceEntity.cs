using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в базе данных
    /// </summary>
    public class FileDataSourceEntity: EntityBase<int>
    {
        /// <summary>
        /// Идентефикатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Имя и расширение файла
        /// </summary>       
        public virtual string FileName { get; set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FileDataEntity FileDataEntity { get; set; }
    }
}
