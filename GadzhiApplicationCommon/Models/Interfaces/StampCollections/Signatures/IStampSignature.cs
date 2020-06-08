using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public interface IStampSignature
    {
        /// <summary>
        /// Имя с идентификатором
        /// </summary>    
        ISignatureLibraryApp SignatureLibrary { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        IResultAppValue<IStampField> Signature { get; }

        /// <summary>
        /// Вертикальное расположение подписи
        /// </summary>
        bool IsVertical { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        bool IsSignatureValid { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        bool IsPersonFieldValid { get; }

        /// <summary>
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public abstract bool IsAbleToInsert { get; }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        IStampSignature InsertSignature(ISignatureFileApp signatureFile);

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        IStampSignature DeleteSignature();
    }
}
