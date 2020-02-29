using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FilesDataEntity : FilesDataEntityBase
    {
        public FilesDataEntity()
        {
            StatusProcessingProject = StatusProcessingProject.InQueue;
        }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public virtual int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataEntity> FileDataEntities { get; protected set; }       

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataEntities(IEnumerable<FileDataEntity> fileDataEntities)
        {
            FileDataEntities = fileDataEntities?.Select(fileData =>
            {
                fileData.FilesDataEntity = this;
                return fileData;
            })?.ToList();
        }      

        /// <summary>
        /// Присовить статус конвертирования
        /// </summary>
        public virtual void StartConverting(string identityServerName)
        {
            StatusProcessingProject = StatusProcessingProject.Converting;
            AttemptingConvertCount += 1;
            IdentityServerName = identityServerName;
        }

        /// <summary>
        /// Присовить статус отмены конвертирования
        /// </summary>
        public virtual void AbortConverting(ClientServer сlientServer)
        {
            switch (сlientServer)
            {
                case ClientServer.Client:
                    StatusProcessingProject = StatusProcessingProject.Abort;                     
                    break;
                case ClientServer.Server:
                    StatusProcessingProject = StatusProcessingProject.InQueue;
                    break;
            }
        }      
    }
}
