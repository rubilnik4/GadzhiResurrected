using System;
using GadzhiDAL.Entities.FilesConvert.Base;

namespace GadzhiDAL.Entities.FilesConvert.ServerStates
{
    /// <summary>
    /// Сущность доступа сервера
    /// </summary>
    public class ServerAccessEntity
    {
        /// <summary>
        /// Имя сервера
        /// </summary>
        public virtual string ServerIdentity { get; set; }

        /// <summary>
        /// Последний доступ
        /// </summary>
        public virtual DateTime LastAccess { get; set; }
    }
}