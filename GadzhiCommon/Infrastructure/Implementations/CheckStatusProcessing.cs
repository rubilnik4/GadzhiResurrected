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
        /// Список статусов, означающих завершенность конвертирования на стороне сервера
        /// </summary>       
        public static IReadOnlyList<StatusProcessingProject> CompletedStatusProcessingProjectServer => new List<StatusProcessingProject>()
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
    }
}
