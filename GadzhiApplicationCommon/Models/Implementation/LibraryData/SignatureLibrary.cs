using System;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibrary: IEquatable<SignatureLibrary>, ISignatureLibrary
    {
        public SignatureLibrary(string id)
            :this(id, String.Empty) { }

        public SignatureLibrary(string id, string fullName)
        {
            PersonId = id ?? throw new ArgumentNullException(nameof(id));
            PersonName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string PersonName { get; }

     
        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((SignatureLibrary)obj);

        public bool Equals(SignatureLibrary other) => other?.PersonId == PersonId;

        public static bool operator ==(SignatureLibrary left, SignatureLibrary right) => left?.Equals(right) == true;

        public static bool operator !=(SignatureLibrary left, SignatureLibrary right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + PersonId.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}