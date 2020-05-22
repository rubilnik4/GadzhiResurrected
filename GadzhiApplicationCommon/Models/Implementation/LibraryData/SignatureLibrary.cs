using System;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibrary: ISignatureLibrary
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
    }
}