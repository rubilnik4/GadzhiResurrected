using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvertingModels.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServer
    {
        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        private readonly List<FileDataServerBase> _fileDatas;

        public FilesDataServer(Guid id,
                               int attemptingConvertCount,
                               IEnumerable<FileDataServerBase> fileDatasServer)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;

            _fileDatas = new List<FileDataServerBase>();
            if (fileDatasServer != null)
            {
                _fileDatas.AddRange(fileDatasServer);
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
        public IReadOnlyList<FileDataServerBase> FileDatas => _fileDatas;

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
            var uncompletedFiles = _fileDatas?.Where(file => !file.IsCompleted);
            foreach (var file in uncompletedFiles)
            {
                file.StatusProcessing = StatusProcessing.ConvertingComplete;
                file.AddFileConvertErrorType(FileConvertErrorType.UnknownError);               
            }
        }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByFileDatas &&
                               IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        public bool IsValidByFileDatas => _fileDatas?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;
    }
}
