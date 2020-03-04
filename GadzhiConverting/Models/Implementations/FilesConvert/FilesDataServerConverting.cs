using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServerConverting : IFilesDataServerConverting
    {
        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        private readonly List<IFileDataServerConverting> _fileDatasServerConverting;

        public FilesDataServerConverting(Guid id, int attemptingConvertCount,
                                         IEnumerable<IFileDataServerConverting> fileDatasServerConverting)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;

            _fileDatasServerConverting = new List<IFileDataServerConverting>();
            if (fileDatasServerConverting != null)
            {
                _fileDatasServerConverting.AddRange(fileDatasServerConverting);
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
        public IReadOnlyList<IFileDataServerConverting> FileDatasServerConverting => _fileDatasServerConverting;

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
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByFileDatas &&
                               IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        public bool IsValidByFileDatas => _fileDatasServerConverting?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;
        
        /// <summary>
        /// Изменить статус обработки для всех файлов
        /// </summary>
        public void SetErrorToAllUncompletedFiles()
        {
            var uncompletedFiles = _fileDatasServerConverting?.Where(file => !file.IsCompleted);
            foreach (var file in uncompletedFiles)
            {
                file.StatusProcessing = StatusProcessing.ConvertingComplete;
                file.AddFileConvertErrorType(FileConvertErrorType.UnknownError);
            }
        }
    }
}
