using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public abstract class FileDataEntityBase : EntityBase<int>
    {
        public FileDataEntityBase()
        {           
            FileConvertErrorType = new List<FileConvertErrorType>();
        }

        /// <summary>
        /// Идентефикатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>       
        public virtual ColorPrint ColorPrint { get; set; }       

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<FileConvertErrorType> FileConvertErrorType { get; set; }
    }
}
