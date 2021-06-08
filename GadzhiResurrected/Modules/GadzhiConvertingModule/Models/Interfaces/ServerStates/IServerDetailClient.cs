using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates
{
    /// <summary>
    /// Информация о сервере. Клиентская модель
    /// </summary>
    public interface IServerDetailClient: IServerDetail<IServerDetailQueue>
    { }
}