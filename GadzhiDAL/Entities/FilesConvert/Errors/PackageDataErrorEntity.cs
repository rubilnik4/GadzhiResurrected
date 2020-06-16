using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Base;

namespace GadzhiDAL.Entities.FilesConvert.Errors
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом пакете с ошибками в базе данных
    /// </summary>
    public class PackageDataErrorEntity : PackageDataEntityBase
    {
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataErrorEntity> FileDataErrorEntities { get; protected set; }

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataEntities(IEnumerable<FileDataErrorEntity> fileDataEntities)
        {
            FileDataErrorEntities = fileDataEntities?.Select(fileData =>
            {
                fileData.PackageDataEntityError = this;
                return fileData;
            }).ToList()
            ?? new List<FileDataErrorEntity>();
        }
    }
}
