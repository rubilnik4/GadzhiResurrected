using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServer
    {
        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        private readonly List<FileDataServer> _filesDataInfo;

        public FilesDataServer(Guid id,
                               int attemptingConvertCount,
                               IEnumerable<FileDataServer> filesDataServer)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;

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
        /// Статус выполнения проекта
        /// </summary>      
        public StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessingProjectServer.Contains(StatusProcessingProject);

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public int AttemptingConvertCount { get; }       

        /// <summary>
        /// Изменить статус обработки для всех файлов
        /// </summary>
        public void SetErrorToAllUncompletedFiles()
        {
            var uncompletedFiles = _filesDataInfo?.Where(file => !file.IsCompleted);
            foreach (var file in uncompletedFiles)
            {
                file.StatusProcessing = StatusProcessing.ConvertingComplete;
                file.AddFileConvertErrorType(FileConvertErrorType.UnknownError);               
            }
        }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByFileData &&
                               IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        public bool IsValidByFileData => _filesDataInfo?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;
    }
}
