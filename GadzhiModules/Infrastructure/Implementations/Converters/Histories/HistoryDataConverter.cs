using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Histories;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels;

namespace GadzhiModules.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование данных истории конвертирования
    /// </summary>
    public static class HistoryDataConverter
    {
        /// <summary>
        /// Преобразовать в запрос
        /// </summary>
        public static HistoryDataRequest ToHistoryDataRequest(DateTime dateTimeFrom, DateTime dateTimeTo, string clientName) =>
            new HistoryDataRequest(dateTimeFrom, dateTimeTo,
                                   (clientName == HistoryFilterViewModel.FILTER_CLIENTS_ALL
                                       ? String.Empty : clientName) ?? String.Empty);

        /// <summary>
        /// Преобразовать ответ в клиентские версии
        /// </summary>
        public static IReadOnlyCollection<IHistoryData> ToClients(IEnumerable<HistoryDataResponse> historyDataResponses) =>
            historyDataResponses.Select(ToClient).ToList();

        /// <summary>
        /// Преобразовать ответ в клиентскую версию
        /// </summary>
        public static IHistoryData ToClient(HistoryDataResponse historyDataResponse) =>
            new HistoryDataClient(historyDataResponse.PackageId, historyDataResponse.CreationDateTime,
                                  historyDataResponse.ClientName, historyDataResponse.StatusProcessingProject,
                                  historyDataResponse.FilesCount);
    }
}