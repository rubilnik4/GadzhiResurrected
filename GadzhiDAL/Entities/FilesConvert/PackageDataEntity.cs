using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.Base;
using GadzhiDAL.Entities.FilesConvert.Components;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом пакете в базе данных
    /// </summary>
    public class PackageDataEntity : EntityBase<string>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override string Id { get; protected set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Время создания запроса на конвертирование
        /// </summary>
        public virtual DateTime CreationDateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Идентификация имени локального пользователя
        /// </summary>
        public virtual string IdentityLocalName { get; set; } = String.Empty;

        /// <summary>
        /// Идентификация имени сервера
        /// </summary>
        public virtual string IdentityServerName { get; set; } = String.Empty;

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public virtual int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataEntity> FileDataEntities { get; protected set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual ConvertingSettingsComponent ConvertingSettings { get; set; }

        /// <summary>
        /// Установить идентификатор
        /// </summary>        
        public virtual void SetId(Guid id)
        {
            Id = id.ToString();
        }

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
