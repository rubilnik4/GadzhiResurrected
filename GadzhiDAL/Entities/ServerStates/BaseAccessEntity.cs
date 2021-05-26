using System;
// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.ServerStates
{
    /// <summary>
    /// Сущность доступа
    /// </summary>
    public abstract class BaseAccessEntity
    {
        protected BaseAccessEntity()
        { }

        protected BaseAccessEntity(string identity, DateTime lastAccess)
        {
            Identity = identity;
            LastAccess = lastAccess;
        }

        /// <summary>
        /// Имя сервера
        /// </summary>
        public virtual string Identity { get; protected set; }

        /// <summary>
        /// Последний доступ
        /// </summary>
        public virtual DateTime LastAccess { get; protected set; }
    }
}