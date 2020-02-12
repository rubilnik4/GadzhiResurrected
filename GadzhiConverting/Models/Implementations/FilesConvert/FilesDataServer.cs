﻿using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.FilesConvert.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServer
    {
        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        private List<FileDataServer> _filesDataInfo;

        public FilesDataServer(Guid id, IEnumerable<FileDataServer> filesDataServer)
        {
            Id = id;

            _filesDataInfo = new List<FileDataServer>();
            if (filesDataServer != null)
            {
                _filesDataInfo.AddRange(filesDataServer);
            }

            StatusProcessingProject = StatusProcessingProject.Converting;
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        public IReadOnlyList<FileDataServer> FilesDataInfo => _filesDataInfo;

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Изменить статус обработки для всех файлов
        /// </summary>
        public void SetStatusToAllFiles(StatusProcessing statusProcessing)
        {
            _filesDataInfo?.ForEach(file =>
            {
                file.StatusProcessing = statusProcessing;
            });
        }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => _filesDataInfo?.Any() == true;
    }
}
