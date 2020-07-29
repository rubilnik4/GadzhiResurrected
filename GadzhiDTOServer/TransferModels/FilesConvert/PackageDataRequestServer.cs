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
        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>   
        [DataMember]
        public int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public override IList<FileDataRequestServer> FilesData { get; set; }

        /// <summary>
        /// Является ли пакет пустым
        /// </summary>
        public bool IsEmptyPackage() => FilesData == null || FilesData.Count == 0;

        /// <summary>
        /// Создать пустой пакет
        /// </summary>
        public static PackageDataRequestServer EmptyPackage => new PackageDataRequestServer();
    }
}
