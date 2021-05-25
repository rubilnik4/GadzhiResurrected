using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемом файле для клиента
    /// </summary>
    [DataContract]
    public class FileDataShortResponseClient : FileDataShortResponseBase
    {
        public FileDataShortResponseClient(string filePath, StatusProcessing statusProcessing,
                                           IList<ErrorCommonResponse> fileErrors)
            :base(filePath, statusProcessing, fileErrors)
        { }
    }
}
