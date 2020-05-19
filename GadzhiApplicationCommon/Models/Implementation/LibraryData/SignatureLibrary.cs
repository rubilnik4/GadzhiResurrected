using System;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibrary: ISignatureLibrary
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
    }
}