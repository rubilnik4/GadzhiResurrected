using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемом файле для сервера
    /// </summary>
    [DataContract]
    public class FileDataShortResponseServer : FileDataShortResponseBase
    {
        public FileDataShortResponseServer(string filePath, StatusProcessing statusProcessing,
                                           IList<ErrorCommonResponse> fileErrors)
            : base(filePath, statusProcessing, fileErrors)
        { }
    }
}
