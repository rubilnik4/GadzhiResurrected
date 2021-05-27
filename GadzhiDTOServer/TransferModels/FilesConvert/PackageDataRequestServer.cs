using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataRequestServer : PackageDataRequestBase<FileDataRequestServer>
    {
        public PackageDataRequestServer(Guid id, ConvertingSettingsRequest convertingSettings,
                                        IList<FileDataRequestServer> filesData, int attemptingConvertCount)
            :base(id, convertingSettings, filesData)
        {
            AttemptingConvertCount = attemptingConvertCount;
        }
        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>   
        [DataMember]
        public int AttemptingConvertCount { get; private set; }

        /// <summary>
        /// Является ли пакет пустым
        /// </summary>
        public bool IsEmptyPackage() =>
            FilesData == null || FilesData.Count == 0;

        /// <summary>
        /// Создать пустой пакет
        /// </summary>
        public static PackageDataRequestServer EmptyPackage =>
            new PackageDataRequestServer(Guid.Empty, null, null, 0);
    }
}
