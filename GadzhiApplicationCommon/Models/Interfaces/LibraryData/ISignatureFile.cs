namespace GadzhiApplicationCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public interface ISignatureFile: ISignatureLibrary
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }
    }
}