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

        public SignatureLibrary(string personId, string personName)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            PersonName = personName ?? throw new ArgumentNullException(nameof(personName));
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