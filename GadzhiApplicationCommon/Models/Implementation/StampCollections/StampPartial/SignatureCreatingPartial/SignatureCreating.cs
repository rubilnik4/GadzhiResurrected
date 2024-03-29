﻿using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Фабрика создания подписей
    /// </summary>
    public abstract class SignatureCreating : ISignatureCreating
    {
        protected SignatureCreating(SignaturesSearching signaturesSearching, string personId, bool useDefaultSignature)
        {
            UseDefaultSignature = useDefaultSignature;
            SignaturesSearching = signaturesSearching ?? throw new ArgumentNullException(nameof(signaturesSearching));
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            UseDefaultSignature = useDefaultSignature;
        }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        protected SignaturesSearching SignaturesSearching { get; }

        /// <summary>
        /// Идентификатор личной подписи
        /// </summary>
        protected string PersonId { get; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        protected bool UseDefaultSignature { get; }

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        public abstract IResultAppCollection<IStampPerson> GetStampPersonRows();

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        public abstract IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary);

        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи
        /// </summary>
        public abstract IResultAppCollection<IStampApproval> GetStampApprovalRows();

        /// <summary>
        /// Получить строки согласования без подписи для извещения с изменениями
        /// </summary>
        public abstract IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows();

        /// <summary>
        /// Получить строки согласования без подписи для опросных листов и тех требований
        /// </summary>
        public abstract IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows();

        /// <summary>
        /// Получить строки согласования без подписи для опросных листов и тех требований с директорами
        /// </summary>
        public abstract IResultAppCollection<IStampApprovalChief> GetStampApprovalChiefRows();

        /// <summary>
        /// Проверка необходимости вставки подписи в строку изменений
        /// </summary>
        protected bool ChangeSignatureValidation(IStampChange stampChange) =>
             !stampChange.DocumentChange.Text.IsNullOrWhiteSpace();
    }
}