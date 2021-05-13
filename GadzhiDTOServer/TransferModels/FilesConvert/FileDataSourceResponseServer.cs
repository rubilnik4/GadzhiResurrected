using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в серверной части
    /// </summary>
    [DataContract]
    public class FileDataSourceResponseServer : FileDataSourceResponseBase
    { }
}
