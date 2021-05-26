using System;

namespace GadzhiDAL.Entities.ServerStates
{
    /// <summary>
    /// Сущность доступа сервера
    /// </summary>
    public class ServerAccessEntity: BaseAccessEntity
    {
        public ServerAccessEntity()
        { }

        public ServerAccessEntity(string identity, DateTime lastAccess)
            : base(identity, lastAccess)
        { }
    }
}