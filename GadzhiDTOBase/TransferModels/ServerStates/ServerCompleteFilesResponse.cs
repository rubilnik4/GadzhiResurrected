using System.Runtime.Serialization;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiDTOBase.TransferModels.ServerStates
{
    /// <summary>
    /// Информация об обработанных файлах на клиенте
    /// </summary>
    [DataContract]
    public class ServerCompleteFilesResponse: IServerCompleteFiles
    {
        public ServerCompleteFilesResponse(int dgnCount, int docCount, int pdfCount, int dwgCount, int xlsCount)
        {
            DgnCount = dgnCount;
            DocCount = docCount;
            PdfCount = pdfCount;
            DwgCount = dwgCount;
            XlsCount = xlsCount;
        }

        /// <summary>
        /// Количество DGN файлов
        /// </summary>
        [DataMember]
        public int DgnCount { get; private set; }

        /// <summary>
        /// Количество DOC файлов
        /// </summary>
        [DataMember]
        public int DocCount { get; private set; }

        /// <summary>
        /// Количество PDF файлов
        /// </summary>
        [DataMember]
        public int PdfCount { get; private set; }

        /// <summary>
        /// Количество DWG файлов
        /// </summary>
        [DataMember]
        public int DwgCount { get; private set; }

        /// <summary>
        /// Количество XLS файлов
        /// </summary>
        [DataMember]
        public int XlsCount { get; private set; }
    }
}