﻿using System;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Фрагмент библиотеки Microstation
    /// </summary>
    public class LibraryElement : IEquatable<LibraryElement>
    {
        public LibraryElement(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Имя фрагмента
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; }

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((LibraryElement)obj);

        public bool Equals(LibraryElement other) => other?.Name == Name && other.Description == Description;

        public static bool operator ==(LibraryElement left, LibraryElement right) => left?.Equals(right) == true;

        public static bool operator !=(LibraryElement left, LibraryElement right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + Name.GetHashCode();
            hashCode = hashCode * 31 + Description.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}
