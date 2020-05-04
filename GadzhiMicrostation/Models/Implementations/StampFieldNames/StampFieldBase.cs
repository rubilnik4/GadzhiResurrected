using System;
using System.Globalization;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Базовые параметры для поля в штампе
    /// </summary>
    public class StampFieldBase : IEquatable<StampFieldBase>
    {
        public StampFieldBase(string name, bool isNeedCompress = true, bool isVertical = false)
        {
            Name = name?.ToUpper(CultureInfo.CurrentCulture);
            IsNeedCompress = isNeedCompress;
            IsVertical = isVertical;
        }

        /// <summary>
        /// Имя поля
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        public bool IsNeedCompress { get; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; }

        #region IEquatable
        public override bool Equals(object obj) => Equals((StampFieldBase)obj);

        public bool Equals(StampFieldBase other) => Name == other?.Name;

        public override int GetHashCode() => Name.GetHashCode();
        #endregion
    }
}
