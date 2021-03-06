﻿using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampTypes;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля основного штампа Word
    /// </summary>
    public class StampFullWord : StampWord, IStampFull
    {
        public StampFullWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching,
                             ITableElementWord tableStamp, IResultAppValue<ITableElementWord> tableApprovalPerformers,
                             IResultAppValue<ITableElementWord> tableApprovalChief)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            TableApprovalPerformers = tableApprovalPerformers ?? throw new ArgumentNullException(nameof(tableApprovalPerformers));
            TableApprovalChief = tableApprovalChief ?? throw new ArgumentNullException(nameof(tableApprovalChief));
        }

        /// <summary>
        /// Элемент таблица согласования списка исполнителей тех требований
        /// </summary>
        protected override IResultAppValue<ITableElementWord> TableApprovalPerformers { get; }

        /// <summary>
        /// Элемент таблица согласования тех требований с директорами
        /// </summary>
        protected override IResultAppValue<ITableElementWord> TableApprovalChief { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Full;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            SignatureCreating.GetStampPersonRows().
            Map(personRows => new StampSignatureFields(new SignaturesBuilder().
                                                       AddStampPersons(personRows).
                                                       AddStampChanges(SignatureCreating.GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary)).
                                                       AddStampApprovalsPerformers(SignatureCreating.GetStampApprovalPerformersRows()).
                                                       AddStampApprovalsChief(SignatureCreating.GetStampApprovalChiefRows())));
    }
}
