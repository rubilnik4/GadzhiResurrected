using GadzhiCommon.Models.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.ServerStates;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates;

namespace GadzhiModules.Infrastructure.Implementations.Converters.ServerStates
{
    /// <summary>
    /// Конвертер информации о серверах на клиенте
    /// </summary>
    public static class ServersInfoConverter
    {
        /// <summary>
        /// Преобразовать информацию серверах в клиентскую модель
        /// </summary>
        public static IServersInfo ToClient(ServersInfoResponse serversInfoResponse) =>
            new ServersInfoClient(serversInfoResponse.ServerNames, serversInfoResponse.CompletePackages,
                                  serversInfoResponse.CompleteFiles, serversInfoResponse.QueuePackages,
                                  serversInfoResponse.QueueFiles);
    }
}