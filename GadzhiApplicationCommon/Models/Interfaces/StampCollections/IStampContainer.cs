using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Контейнер штампов
    /// </summary>
    public interface IStampContainer
    {
        /// <summary>
        /// Тип контейнера штампов
        /// </summary>
        StampContainerType StampContainerType { get; }

        /// <summary>
        /// Получить штампы для печати
        /// </summary>
        IResultAppCollection<IStamp> GetStampsToPrint();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        IResultAppCollection<IStampSignature> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures);
    }
}