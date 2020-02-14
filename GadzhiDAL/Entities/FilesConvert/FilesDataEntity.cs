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
    public class FilesDataEntity : EntityBase<string>
    {
        public FilesDataEntity()
        {
            CreationDateTime = DateTime.Now;
            IsCompleted = false;
            StatusProcessingProject = StatusProcessingProject.InQueue;
            FilesData = new List<FileDataEntity>();
        }

        /// <summary>
        /// Идентефикатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Время создания запроса на конвертирование
        /// </summary>
        public virtual DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Идентефикация пользователя
        /// </summary>
        public virtual string IdentityName { get; set; }

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
        public virtual IList<FileDataEntity> FilesData { get; protected set; }

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFilesData(IEnumerable<FileDataEntity> fileDataEntities)
        {
            if (fileDataEntities?.Any() == true)
            {
                FilesData = fileDataEntities?.Select(fileData =>
                                             {
                                                 fileData.FilesDataEntity = this;
                                                 return fileData;
                                             })
                                             .ToList();
            }
            else
            {
                fileDataEntities = new List<FileDataEntity>();
            }
        }

        /// <summary>
        /// Установить идентефикатор
        /// </summary>        
        public virtual void SetId(Guid id)
        {
            Id = id.ToString();
        }
    }
}
