﻿using System;

namespace GadzhiCommon.Models.Implementations.Functional
{
    /// <summary>
    /// Тип Unit для замены void
    /// </summary>
    public struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// Получить тип Unit
        /// </summary>
        public static Unit Value { get; } = new Unit();

        #region IEquatable
        public bool Equals(Unit other) => true;

        public override int GetHashCode() => 0;

        public override bool Equals(object obj) => obj is Unit;

        public override string ToString() => "Unit";

        public static bool operator ==(Unit left, Unit right) => left.Equals(right);

        public static bool operator !=(Unit left, Unit right) => !(left == right);
        #endregion
    }
}
