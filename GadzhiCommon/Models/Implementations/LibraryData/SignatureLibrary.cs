using System;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibrary: ISignatureLibrary, IEquatable<ISignatureLibrary>
    {

        public SignatureLibrary(string personId, PersonInformation personInformation)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            PersonInformation = personInformation;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Имя 
        /// </summary>
        public PersonInformation PersonInformation { get; }

        #region IEquatable
        public override bool Equals(object obj) => obj is ISignatureLibrary signatureLibrary && Equals(signatureLibrary);

        public bool Equals(ISignatureLibrary other) => other?.PersonId == PersonId;

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + PersonId.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}