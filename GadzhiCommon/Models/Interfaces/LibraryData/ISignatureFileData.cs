namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public interface ISignatureFileData: ISignatureLibrary
    {
        /// <summary>
        /// Изображение подписи
        /// </summary>
        byte[] SignatureFileDataSource { get; }
    }
}