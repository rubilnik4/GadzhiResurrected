using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для сервера
    /// </summary>
    [DataContract]
    public class FileDataRequestServer : FileDataRequestBase
    {
        public FileDataRequestServer(string filePath, ColorPrintType colorPrintType, StatusProcessing statusProcessing,
                                        byte[] fileDataSource, string fileExtensionAdditional, byte[] fileDataSourceAdditional)
        :base(filePath, colorPrintType, statusProcessing, fileDataSource, fileExtensionAdditional, fileDataSourceAdditional)
        { }
    }
}
