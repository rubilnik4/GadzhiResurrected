using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для сервера
    /// </summary>
    [DataContract]
    public class FileDataResponseServer: FileDataResponseBase
    {

    }
}
