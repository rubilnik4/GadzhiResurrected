using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Идентификатор штампа
    /// </summary>
    public readonly struct StampIdentifier : IEquatable<StampIdentifier>
    {
        public StampIdentifier(int stampIndex)
            : this(0, stampIndex) { }

        public StampIdentifier(int modelIndex, int stampIndex)
        {
            if (modelIndex < 0) throw new ArgumentOutOfRangeException(nameof(modelIndex));
            if (stampIndex < 0) throw new ArgumentOutOfRangeException(nameof(stampIndex));

            ModelIndex = modelIndex;
            StampIndex = stampIndex;
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Порядковый номер модели
        /// </summary>
        public int ModelIndex { get; }

        /// <summary>
        /// Порядковый номер штампа
        /// </summary>
        public int StampIndex { get; }

        /// <summary>
        /// Преобразовать в имя пути для сохранения файла
        /// </summary>
        public string ToFilePathPrefix() =>
            ModelIndex switch
            {
                0 when StampIndex == 0 => String.Empty,
                0 => $" ({StampIndex})",
                _ => $" ({ModelIndex}; {StampIndex + 1})"
            };

        #region IEquatable

        public override bool Equals(object obj) => obj != null && Equals((StampIdentifier)obj);

        public bool Equals(StampIdentifier other) => Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(StampIdentifier left, StampIdentifier right) => left.Equals(right);

        public static bool operator !=(StampIdentifier left, StampIdentifier right) => !(left == right);
        #endregion
    }
}
