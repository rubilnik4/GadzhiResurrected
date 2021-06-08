using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.Histories;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.Histories;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование файлов истории конвертирования
    /// </summary>
    public static class HistoryFileDataConverter
    {
        /// <summary>
        /// Преобразовать ответ в клиентские версии
        /// </summary>
        public static IReadOnlyCollection<IHistoryFileDataClient> ToClients(IEnumerable<HistoryFileDataResponse> historyFileDataResponses) =>
            historyFileDataResponses.Select(ToClient).ToList();

        /// <summary>
        /// Преобразовать ответ в клиентскую версию
        /// </summary>
        public static IHistoryFileDataClient ToClient(HistoryFileDataResponse historyFileDataResponse) =>
            new HistoryFileDataClient(historyFileDataResponse.FilePath, historyFileDataResponse.StatusProcessing,
                                      historyFileDataResponse.ColorPrintType,
                                      historyFileDataResponse.HistoryFileDataSources.Select(ToSourceClient), 
                                      historyFileDataResponse.ErrorCount);

        /// <summary>
        /// Преобразовать ответ в клиентскую версию
        /// </summary>
        public static IHistoryFileDataSource ToSourceClient(HistoryFileDataSourceResponse historyFileDataSourceResponse) =>
            new HistoryFileDataSourceClient(historyFileDataSourceResponse.FileName, historyFileDataSourceResponse.FileExtensionType,
                                            historyFileDataSourceResponse.PrinterName, historyFileDataSourceResponse.PaperSize);


    }
}