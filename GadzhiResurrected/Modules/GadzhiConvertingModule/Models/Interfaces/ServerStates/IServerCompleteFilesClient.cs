﻿using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates
{

    /// <summary>
    /// Информация об обработанных файлах на клиенте
    /// </summary>
    public interface IServerCompleteFilesClient : IServerCompleteFiles
    {
        /// <summary>
        /// Всего
        /// </summary>
        int TotalCount { get; }
    }
}