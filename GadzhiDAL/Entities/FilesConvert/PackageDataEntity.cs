using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Components;
// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом пакете в базе данных
    /// </summary>
    public class PackageDataEntity
    {
        public PackageDataEntity()
        { }

        public PackageDataEntity(string id, StatusProcessingProject statusProcessingProject, DateTime creationDateTime, 
                                 string identityLocalName, string identityServerName, int attemptingConvertCount, 
                                 IList<FileDataEntity> fileDataEntities, ConvertingSettingsComponent convertingSettings)
        {
            Id = id;
            StatusProcessingProject = statusProcessingProject;
            CreationDateTime = creationDateTime;
            IdentityLocalName = identityLocalName;
            IdentityServerName = identityServerName;
            AttemptingConvertCount = attemptingConvertCount;
            FileDataEntities = fileDataEntities;
            ConvertingSettings = convertingSettings;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string Id { get; protected set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Время создания запроса на конвертирование
        /// </summary>
        public virtual DateTime CreationDateTime { get; protected set; } = DateTime.MinValue;

        /// <summary>
        /// Идентификация имени локального пользователя
        /// </summary>
        public virtual string IdentityLocalName { get; protected set; } 

        /// <summary>
        /// Идентификация имени сервера
        /// </summary>
        public virtual string IdentityServerName { get; protected set; } 

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public virtual int AttemptingConvertCount { get; protected set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual IList<FileDataEntity> FileDataEntities { get; set; }

        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>       
        public virtual ConvertingSettingsComponent ConvertingSettings { get; protected set; }

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
