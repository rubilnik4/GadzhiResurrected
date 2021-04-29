using System;

namespace GadzhiDAL.Entities.ServerStates
{
    /// <summary>
    /// Сущность доступа
    /// </summary>
    public abstract class BaseAccessEntity
    {
        /// <summary>
        /// Имя сервера
        /// </summary>
        public virtual string Identity { get; set; }

        /// <summary>
        /// Последний доступ
        /// </summary>
        public virtual DateTime LastAccess { get; set; }
    }
}