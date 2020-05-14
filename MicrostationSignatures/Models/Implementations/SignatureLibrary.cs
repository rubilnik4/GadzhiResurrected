using System;

namespace MicrostationSignatures.Models.Implementations
{
    public readonly struct SignatureLibrary : IEquatable<SignatureLibrary>
    {
        public SignatureLibrary(string name, string description)
        {
            Id = name;
            Fullname = description;
        }

        /// <summary>
        /// Имя фрагмента
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Fullname { get; }

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((SignatureLibrary)obj);

        public bool Equals(SignatureLibrary other) => other.Id == Id;

        public static bool operator ==(SignatureLibrary left, SignatureLibrary right) => left.Equals(right);

        public static bool operator !=(SignatureLibrary left, SignatureLibrary right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + Id.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}