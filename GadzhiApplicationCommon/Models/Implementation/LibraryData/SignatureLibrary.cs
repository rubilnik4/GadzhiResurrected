using System;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и сохраненным файлом изображения
    /// </summary>
    public class SignatureLibrary: IEquatable<SignatureLibrary>
    {
        public SignatureLibrary(string id)
            :this(id, String.Empty) { }

        public SignatureLibrary(string id, string fullName)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Fullname = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string Fullname { get; }

     
        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((SignatureLibrary)obj);

        public bool Equals(SignatureLibrary other) => other?.Id == Id;

        public static bool operator ==(SignatureLibrary left, SignatureLibrary right) => left?.Equals(right) == true;

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