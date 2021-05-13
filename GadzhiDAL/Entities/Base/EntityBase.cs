using System;

namespace GadzhiDAL.Entities.Base
{
    /// <summary>
    /// Базовый класс для сущностей
    /// </summary>
    public abstract class EntityBase<TIdType> where TIdType : IEquatable<TIdType>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public abstract TIdType Id { get; protected set; }
    }
}
