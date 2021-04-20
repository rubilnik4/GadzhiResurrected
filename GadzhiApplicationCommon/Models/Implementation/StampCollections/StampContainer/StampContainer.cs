using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampContainer
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
        /// Тип документа, определяемый по типу шифра в штампе
        /// </summary>
        public StampDocumentType StampDocumentType =>
            _stamps.WhereContinue(stamps => stamps.OkStatus && stamps.Value.Count > 0 &&
                                            stamps.Value[0].StampBasicFields.FullCode.OkStatus,
                okFunc: stamps => stamps.Value[0].StampDocumentType,
                badFunc: _ => StampDocumentType.Unknown);

        /// <summary>
        /// Штампы для печати
        /// </summary>
        public IResultAppCollection<IStamp> GetStampsToPrint() =>
            StampContainerType switch
            {
                StampContainerType.Separate => _stamps,
                StampContainerType.United => _stamps.
                                             ResultValueOk(stamps => stamps.Where(stamp => stamp.IsStampTypeMain)).
                                             ToResultCollection().
                                             ResultValueContinue(stamps => stamps.Count > 0, 
                                                okFunc: stamps => stamps,
                                                badFunc: _ => new ErrorApplication(ErrorApplicationType.StampNotFound, "Основные штампы не найдены")).
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
            ResultValueOkBind(stamps => stamps.Select(stamp => signatures.Where(signature => signature.StampIdentifier.Equals(stamp.StampSettings.Id)).
                                                                          Map(stamp.DeleteSignatures) ).
                                               ToResultCollection()).
            ToResultCollection();
    }
}