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
            FilesData = new List<FileDataEntity>();
        }

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
            FilesData = fileDataEntities?.Select(fileDataEntity =>
            {
                fileDataEntity.FilesDataEntity = this;
                return fileDataEntity;
            })?.ToList();
        }

        /// <summary>
        /// Установить идентефикатор
        /// </summary>        
        public virtual void SetId(Guid id)
        {
            Id = id.ToString();
        }

        /// <summary>
        /// Присовить статус конвертирования
        /// </summary>
        public virtual void StartConverting(string identityServerName)
        {
            StatusProcessingProject = StatusProcessingProject.Converting;
            IdentityMachine.AttemptingConvertCount += 1;
            IdentityMachine.IdentityServerName = identityServerName;
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
