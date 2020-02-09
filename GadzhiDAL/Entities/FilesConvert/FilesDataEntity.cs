using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FilesDataEntity : EntityBase
    {
       public FilesDataEntity()
        {
            FilesData = new List<FileDataEntity>();
        }

        /// <summary>
        /// Идентефикатор
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// ID идентефикатор
        /// </summary>        
        public virtual Guid IdGuid { get; set; }        

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public virtual bool IsCompleted { get; set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataEntity> FilesData { get; set; }
    }
}
