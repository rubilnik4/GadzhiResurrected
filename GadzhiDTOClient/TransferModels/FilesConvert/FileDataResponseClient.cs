using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для клиента
    /// </summary>
    [DataContract]
    public class FileDataResponseClient : FileDataResponseBase<FileDataSourceResponseClient>
    {
        public FileDataResponseClient(string filePath, StatusProcessing statusProcessing,
                                      IList<ErrorCommonResponse> fileErrors, IList<FileDataSourceResponseClient> filesDataSource)
            : base(filePath, statusProcessing, fileErrors, filesDataSource)
        { }
    }
}
