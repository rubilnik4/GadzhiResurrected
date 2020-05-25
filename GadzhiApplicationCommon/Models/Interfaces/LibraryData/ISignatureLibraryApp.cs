using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public interface ISignatureLibraryApp
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        PersonInformationApp PersonInformation { get; }
    }
}