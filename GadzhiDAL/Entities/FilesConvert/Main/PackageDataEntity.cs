﻿using GadzhiCommon.Enums.FilesConvert;
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
    public class PackageDataEntity : PackageDataEntityBase
    {
        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public virtual StatusProcessingProject StatusProcessingProject { get; set; } = StatusProcessingProject.InQueue;

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
                fileData.PackageDataEntity = this;
                return fileData;
            }).ToList()
            ?? new List<FileDataEntity>();
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