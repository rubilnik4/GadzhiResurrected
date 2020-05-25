namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public interface ISignatureFile: ISignatureLibrary
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        string SignatureFilePath { get; }
    }
}