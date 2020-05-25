using GadzhiCommon.Models.Implementations.LibraryData;

namespace GadzhiCommon.Models.Interfaces.LibraryData
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
        /// Информация о пользователе
        /// </summary>
        PersonInformation PersonInformation { get; }
    }
}