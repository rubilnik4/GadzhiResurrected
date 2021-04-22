using GadzhiCommon.Models.Implementations.ServerStates;
using GadzhiCommon.Models.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.ServerStates;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;

namespace GadzhiModules.Infrastructure.Implementations.Converters.ServerStates
{
    /// <summary>
    /// Преобразовать информацию о сервере в клиентскую модель
    /// </summary>
    public static class ServerDetailConverter
    {
        public static IServerDetailClient ToClient(ServerDetailResponse serverDetailResponse) =>
            new ServerDetailClient(serverDetailResponse.ServerName, serverDetailResponse.LastAccess,
                                   ToClient(serverDetailResponse.ServerDetailQueue),
                                   serverDetailResponse.ServerActivityType);

        public static IServerDetailQueue ToClient(ServerDetailQueueResponse serverDetailQueueResponse) =>
            new ServerDetailQueue(serverDetailQueueResponse.CurrentUser, serverDetailQueueResponse.CurrentPackage,
                                  serverDetailQueueResponse.CurrentFile, serverDetailQueueResponse.FilesInQueue,
                                  serverDetailQueueResponse.PackagesComplete, serverDetailQueueResponse.FilesComplete);
    }
}