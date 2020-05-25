using System;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public class SignatureLibraryApp: ISignatureLibraryApp
    {

        public SignatureLibraryApp(string personId, PersonInformationApp personInformation)
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
        public PersonInformationApp PersonInformation { get; }
    }
}