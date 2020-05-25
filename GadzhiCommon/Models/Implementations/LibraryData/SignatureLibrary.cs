using System;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
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