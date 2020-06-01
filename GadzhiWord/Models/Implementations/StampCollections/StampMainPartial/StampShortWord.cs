﻿using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Поля сокращенного штампа Word
    /// </summary
    public class StampShortWord : StampWord, IStampMain
    {
        public StampShortWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {

        }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        private IResultAppCollection<IStampChange> _stampChanges;

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IResultAppCollection<IStampChange> StampChanges =>
            _stampChanges ??= GetStampChangeRows(StampPersons.Value?.FirstOrDefault()?.SignatureLibrary);

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Shortened;

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> InsertSignatures() =>
            GetSignatures(StampPersons, StampChanges, new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>())).
                ResultValueOkBind(GetStampSignaturesByIds).
                ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.
                Select(signature => signature.DeleteSignature()).
                ToList().
                Map(signaturesDeleted => new ResultAppCollection<IStampSignature>
                        (signaturesDeleted,
                         signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                         new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для удаления не инициализированы")));
    }
}