using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Контейнер штампов
    /// </summary>
    public class StampContainer : IStampContainer
    {

        /// <summary>
        /// Штампы
        /// </summary>
        private readonly IResultAppCollection<IStamp> _stamps;

        public StampContainer(IResultAppCollection<IStamp> stamps, StampContainerType stampContainerType)
        {
            _stamps = stamps ?? throw new ArgumentNullException(nameof(stamps));
            StampContainerType = stampContainerType;
        }

        /// <summary>
        /// Тип контейнера штампов
        /// </summary>
        public StampContainerType StampContainerType { get; }

        /// <summary>
        /// Штампы для печати
        /// </summary>
        public IResultAppCollection<IStamp> GetStampsToPrint() =>
            StampContainerType switch
            {
                StampContainerType.Separate => _stamps,
                StampContainerType.United => _stamps.
                                             ResultValueOk(stamps => stamps.Where(stamp => stamp.StampType == StampType.Main)).
                                             ToResultCollection(),
                _ => throw new InvalidEnumArgumentException(nameof(StampContainerType), (int)StampContainerType, typeof(StampContainerType))
            };

        /// <summary>
        /// Сжать поля
        /// </summary>
        public IResultAppCollection<bool> CompressFieldsRanges() =>
            _stamps.
            ResultValueOk(stamps => stamps.SelectMany(stamp => stamp.CompressFieldsRanges())).
            ToResultCollection();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public IResultAppCollection<IStampSignature> InsertSignatures() =>
            _stamps.
            ResultValueOkBind(stamps => stamps.Select(stamp => stamp.InsertSignatures()).
                                               ToResultCollection()).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            _stamps.
            ResultValueOkBind(stamps => stamps.Select(stamp => stamp.DeleteSignatures(signatures)).
                                               ToResultCollection()).
            ToResultCollection();
    }
}