using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в серверной части
    /// </summary>
    [DataContract]
    public class FileDataSourceResponseServer : FileDataSourceResponseBase
    {
        public FileDataSourceResponseServer(string fileName, FileExtensionType fileExtensionType, string paperSize,
                                            string printerName, byte[] fileDataSource)
            : base(fileName, fileExtensionType, paperSize, printerName, fileDataSource)
        { }
    }
}
