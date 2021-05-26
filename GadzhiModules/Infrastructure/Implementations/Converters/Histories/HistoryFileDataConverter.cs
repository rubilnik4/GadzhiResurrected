using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Histories;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.Histories;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels;

namespace GadzhiModules.Infrastructure.Implementations.Converters.Histories
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
                                      historyFileDataResponse.HistoryFileDataSources.Select(ToSourceClient), 
                                      historyFileDataResponse.ErrorCount);

        /// <summary>
        /// Преобразовать ответ в клиентскую версию
        /// </summary>
        public static IHistoryFileDataSource ToSourceClient(HistoryFileDataSourceResponse historyFileDataSourceResponse) =>
            new HistoryFileDataSourceClient(historyFileDataSourceResponse.FileExtensionType,
                                            historyFileDataSourceResponse.PrinterName,
                                            historyFileDataSourceResponse.PaperSize);


    }
}