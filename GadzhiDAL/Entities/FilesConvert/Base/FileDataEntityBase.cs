using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Base.Components;

namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public abstract class FileDataEntityBase <TSourceEntity> : EntityBase<int>
        where TSourceEntity: FileDataSourceEntityBase
    {       
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>       
        public virtual ColorPrintType ColorPrintType { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<TSourceEntity> FileDataSourceServerEntities { get; protected set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrors { get; set; }
    }
}
