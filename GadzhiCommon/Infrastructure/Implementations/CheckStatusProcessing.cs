using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Операции над статусом обработки пакета
    /// </summary>
    public static class CheckStatusProcessing
    {
        /// <summary>
        /// Список статусов, означающих завершенность конвертирования
        /// </summary>       
        public static IReadOnlyList<StatusProcessingProject> CompletedStatusProcessingProject => new List<StatusProcessingProject>()
        {
            StatusProcessingProject.ConvertingComplete,
            StatusProcessingProject.Error,
            StatusProcessingProject.Abort,
            StatusProcessingProject.End,
            StatusProcessingProject.Archived,
        };

        /// <summary>
        /// Список статусов, означающих взятых в процесс конвертирования
        /// </summary>       
        public static IReadOnlyList<StatusProcessingProject> OperatingStatusProcessingProject =>
            CompletedStatusProcessingProject.
            Concat(new List<StatusProcessingProject> { StatusProcessingProject.InQueue }).
            ToList();

        /// <summary>
        /// Список статусов, означающих завершенность конвертирования файла на стороне сервера
        /// </summary>
        public static IReadOnlyList<StatusProcessing> CompletedStatusProcessing => new List<StatusProcessing>()
        {
            StatusProcessing.ConvertingComplete,
            StatusProcessing.Writing,
            StatusProcessing.End,
            StatusProcessing.Archive,
        };
    }
}
