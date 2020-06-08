namespace GadzhiApplicationCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public interface ISignatureFileApp: ISignatureLibraryApp
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        string SignatureFilePath { get; }

        /// <summary>
        /// Вертикальное расположение изображения
        /// </summary>
        bool IsVerticalImage { get; }
    }
}