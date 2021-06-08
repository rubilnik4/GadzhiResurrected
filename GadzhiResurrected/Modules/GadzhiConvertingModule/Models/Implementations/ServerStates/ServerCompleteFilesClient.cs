using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates
{
    /// <summary>
    /// Информация об обработанных файлах на клиенте
    /// </summary>
    public class ServerCompleteFilesClient: IServerCompleteFilesClient
    {
        public ServerCompleteFilesClient(int dgnCount, int docCount, int pdfCount, int dwgCount, int xlsCount)
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
        public int DgnCount { get; }

        /// <summary>
        /// Количество DOC файлов
        /// </summary>
        public int DocCount { get; }

        /// <summary>
        /// Количество PDF файлов
        /// </summary>
        public int PdfCount { get; }

        /// <summary>
        /// Количество DWG файлов
        /// </summary>
        public int DwgCount { get; }

        /// <summary>
        /// Количество XLS файлов
        /// </summary>
        public int XlsCount { get; }

        /// <summary>
        /// Всего
        /// </summary>
        public int TotalCount => 
            DgnCount + DocCount + PdfCount + DwgCount + XlsCount;
    }
}