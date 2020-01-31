using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Models.FilesConvert.Implementations
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
            ID = id;

            _filesDataInfo = new List<FileDataServer>();
            if (filesDataServer != null)
            {
                _filesDataInfo.AddRange(filesDataServer);
            }
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>
        public Guid ID { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        public IReadOnlyList<FileDataServer> FilesDataInfo => _filesDataInfo;

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public bool IsComplited { get; set; }

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

    }
}
