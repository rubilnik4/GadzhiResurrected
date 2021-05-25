using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для сервера
    /// </summary>
    [DataContract]
    public class FileDataResponseServer : FileDataResponseBase<FileDataSourceResponseServer>
    {
        public FileDataResponseServer(string filePath, StatusProcessing statusProcessing, IList<ErrorCommonResponse> fileErrors,
                                      IList<FileDataSourceResponseServer> filesDataSource)
          : base(filePath, statusProcessing, fileErrors, filesDataSource)
        { }
    }
}
