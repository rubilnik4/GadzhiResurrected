using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Операции над статусом обработки пакета
    /// </summary>
    public static class CheckStatusProcessing
    {
        /// <summary>
        /// Список статусов, означающих завершенность конвертирования на стороне сервера
        /// </summary>       
        public static IReadOnlyList<StatusProcessingProject> CompletedStatusProcessingProject => new List<StatusProcessingProject>()
        {
            StatusProcessingProject.ConvertingComplete,
            StatusProcessingProject.Error,
            StatusProcessingProject.Abort,
        };

        /// <summary>
        /// Список статусов, означающих завершенность конвертирования файла на стороне сервера
        /// </summary>
        public static IReadOnlyList<StatusProcessing> CompletedStatusProcessingServer => new List<StatusProcessing>()
        {
            StatusProcessing.ConvertingComplete,
        };

        /// <summary>
        /// Находится ли статус в списке завершенных на стороне севера
        /// </summary>
        public static bool IsCompletedStatusProcessingProject(StatusProcessingProject statusProcessingProject) =>
            CompletedStatusProcessingProject.Contains(statusProcessingProject);

        /// <summary>
        /// Находится ли статус в списке завершенных на стороне севера
        /// </summary>
        public static bool IsCompletedStatusProcessingServer(StatusProcessing statusProcessing) =>
            CompletedStatusProcessingServer.Contains(statusProcessing);
    }
}
