using System;

namespace GadzhiDAL.Entities
{
    /// <summary>
    /// Базовый класс для сущностей
    /// </summary>
    public abstract class EntityBase<IdType> where IdType : IEquatable<IdType>
    {
        /// <summary>
        /// Идентефикатор
        /// </summary>
        public abstract IdType Id { get; protected set; }
    }
}
