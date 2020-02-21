using System;
using System.Globalization;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Базовые параметры для поля в штампе
    /// </summary>
    public class StampBaseField : IEquatable<StampBaseField>
    {
        public StampBaseField(string name,
                              bool isNeedCompress = true,
                              bool isVertical = false)
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

        public override bool Equals(object obj)
        {
            return Equals((StampBaseField)obj);
        }

        public bool Equals(StampBaseField other)
        {
            return Name == other?.Name;
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}
