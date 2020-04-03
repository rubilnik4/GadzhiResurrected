using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Functional
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

        public bool Equals(Unit other) => true;

        public override int GetHashCode() => 0;

        public override bool Equals(object obj) => obj is Unit;

        public override string ToString() => "Unit";

        public static bool operator ==(Unit left, Unit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Unit left, Unit right)
        {
            return !(left == right);
        }
    }
}
