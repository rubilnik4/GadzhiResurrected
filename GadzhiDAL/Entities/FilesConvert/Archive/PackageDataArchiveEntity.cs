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
    public class PackageDataArchiveEntity : PackageDataEntityBase<FileDataArchiveEntity, FileDataSourceArchiveEntity>
    {
        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataArchiveEntities(IEnumerable<FileDataArchiveEntity> fileDataArchiveEntities)
        {
            FileDataEntities = fileDataArchiveEntities?.Select(fileDataArchive =>
            {
                fileDataArchive.PackageDataArchiveEntity = this;
                return fileDataArchive;
            }).ToList();
        } 
    }
}
