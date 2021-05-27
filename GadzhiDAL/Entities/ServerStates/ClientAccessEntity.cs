using System;

namespace GadzhiDAL.Entities.ServerStates
{
    /// <summary>
    /// Сущность доступа клиента
    /// </summary>
    public class ClientAccessEntity : BaseAccessEntity
    {
        public ClientAccessEntity()
        { }

        public ClientAccessEntity(string identity, DateTime lastAccess)
            :base(identity, lastAccess)
        { }
    }
}