using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Archive
{
    /// <summary>
    /// Конвертер в архивную версию
    /// </summary>    
    public static class ConverterToArchive
    {
        /// <summary>
        /// Конвертировать пакет в архивную версию базы данных
        /// </summary>    
        public static PackageDataEntity PackageDataToArchive(PackageDataEntity packageDataEntity)
        {
            packageDataEntity.StatusProcessingProject = StatusProcessingProject.Archived;
            foreach (var fileDataEntity in packageDataEntity.FileDataEntities)
            {
                fileDataEntity.StatusProcessing = StatusProcessing.Archive;
                fileDataEntity.FileDataSource = null;
                fileDataEntity.FileDataSourceAdditional = null;
                foreach (var fileDataSourceEntity in fileDataEntity.FileDataSourceServerEntities)
                {
                    fileDataSourceEntity.FileDataSource = null;
                }
            }
            return packageDataEntity;
        }
    }
}
