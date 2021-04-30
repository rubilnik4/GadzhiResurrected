using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом пакете в базе данных
    /// </summary>
    public class PackageDataEntity : PackageDataEntityBase
    {
        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public virtual int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual ConvertingSettingsComponent ConvertingSettings { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataEntity> FileDataEntities { get; protected set; }

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataEntities(IEnumerable<FileDataEntity> fileDataEntities)
        {
            FileDataEntities = fileDataEntities.Select(fileData =>
            {
                fileData.PackageDataEntity = this;
                return fileData;
            }).ToList();
        }

        /// <summary>
        /// Присвоить статус конвертирования
        /// </summary>
        public virtual void StartConverting(string identityServerName)
        {
            StatusProcessingProject = StatusProcessingProject.Converting;
            AttemptingConvertCount += 1;
            IdentityServerName = identityServerName;
        }

        /// <summary>
        /// Присвоить статус отмены конвертирования, если файл необработан
        /// </summary>
        public virtual void AbortConverting(ClientServer clientServer)
        {
            if (StatusProcessingProject != StatusProcessingProject.ConvertingComplete)
            {
                StatusProcessingProject = clientServer switch
                {
                    ClientServer.Client => StatusProcessingProject.Abort,
                    ClientServer.Server => StatusProcessingProject.InQueue,
                    _ => StatusProcessingProject
                };
            }
        }
    }
}
