using System;

namespace GadzhiApplicationCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public interface ISignatureLibrary
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Имя 
        /// </summary>
        string PersonName { get; }
    }
}