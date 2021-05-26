namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public interface ISignatureFileBase<out TPerson> : ISignatureLibraryBase<TPerson>
        where TPerson : IPersonInformation
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        string SignatureFilePath { get; }
    }
}