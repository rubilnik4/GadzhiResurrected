namespace GadzhiCommon.Models.Interfaces.ServerStates
{
    /// <summary>
    /// Информация об обработанных файлах
    /// </summary>
    public interface IServerCompleteFiles
    {
        /// <summary>
        /// Количество DGN файлов
        /// </summary>
        int DgnCount { get; }

        /// <summary>
        /// Количество DOC файлов
        /// </summary>
        int DocCount { get; }

        /// <summary>
        /// Количество PDF файлов
        /// </summary>
        int PdfCount { get; }

        /// <summary>
        /// Количество DWG файлов
        /// </summary>
        int DwgCount { get; }

        /// <summary>
        /// Количество XLS файлов
        /// </summary>
        int XlsCount { get; }
    }
}