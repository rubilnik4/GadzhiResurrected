using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Archive
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FilesDataArchiveEntity : FilesDataEntityBase
    { 
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataArchiveEntity> FileDataArchiveEntities { get; protected set; }       

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataArchiveEntities(IEnumerable<FileDataArchiveEntity> fileDataArchiveEntities)
        {
            FileDataArchiveEntities = fileDataArchiveEntities?.Select(fileDataArchive =>
            {
                fileDataArchive.FilesDataArchiveEntity = this;
                return fileDataArchive;
            })?.ToList();
        } 
    }
}
