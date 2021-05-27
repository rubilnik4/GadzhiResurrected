using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для клиента
    /// </summary>
    [DataContract]
    public class FileDataRequestClient : FileDataRequestBase
    { 
        public FileDataRequestClient(string filePath, ColorPrintType colorPrintType, StatusProcessing statusProcessing,
                                     byte[] fileDataSource, string fileExtensionAdditional, byte[] fileDataSourceAdditional)
            :base(filePath, colorPrintType, statusProcessing, fileDataSource, fileExtensionAdditional, fileDataSourceAdditional)
        { }
    }
}
