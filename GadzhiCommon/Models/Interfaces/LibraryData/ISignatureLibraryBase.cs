using GadzhiCommon.Models.Implementations.LibraryData;

namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором
    /// </summary>
    public interface ISignatureLibraryBase<out TPerson>
        where TPerson: IPersonInformation
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        TPerson PersonInformation { get; }
    }
}