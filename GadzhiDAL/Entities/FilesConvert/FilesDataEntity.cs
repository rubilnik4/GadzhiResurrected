using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IdentityMachine = new IdentityMachine()
            {
                AttemptingConvertCount = 0,
                IdentityLocalName = "",
                IdentityServerName = "",
            };
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
        /// Завершена ли обработка
        /// </summary>
        public virtual bool IsCompleted { get; set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Идентефикация устройства
        /// </summary>
        public virtual IdentityMachine IdentityMachine { get; set; }

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
                    IsCompleted = true;
                    StatusProcessingProject = StatusProcessingProject.Error;
                    break;
                case ClientServer.Server:
                    IsCompleted = false;
                    StatusProcessingProject = StatusProcessingProject.InQueue;
                    break;
            }
        }
    }
}
