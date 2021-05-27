using System;
using System.Globalization;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibrary: ISignatureLibrary, IEquatable<ISignatureLibrary>, IFormattable
    {

        public SignatureLibrary(string personId, PersonInformation personInformation)
        {
            PersonId = personId;
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

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => PersonInformation.FullInformation;
        #endregion

        #region IEquatable
        public override bool Equals(object obj) => 
            obj is ISignatureLibrary signatureLibrary && Equals(signatureLibrary);

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