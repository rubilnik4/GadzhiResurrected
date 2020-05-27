using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public partial class StampMainMicrostation : StampMicrostation, IStampMain
    {
        public StampMainMicrostation(ICellElementMicrostation stampCellElement, StampSettings stampSettings,
                                     SignaturesSearching signaturesSearching)
            : base(stampSettings, stampCellElement, signaturesSearching)
        {
            StampPersons = GetStampPersonRows();
            StampChanges = GetStampChangeRows(StampPersons.Value?.FirstOrDefault()?.SignatureLibrary);
            StampApprovals = GetStampApprovalRows();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        public IResultAppCollection<IStampPerson> StampPersons { get; }

        /// <summary>
        /// Строки с изменениями
        /// </summary>
        public IResultAppCollection<IStampChange> StampChanges { get; }

        /// <summary>
        /// Строки с согласованиями
        /// </summary>
        public IResultAppCollection<IStampApproval> StampApprovals { get; }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IResultAppCollection<IStampSignature> InsertSignaturesFromLibrary(IList<LibraryElement> libraryElements) =>
            GetSignatures(StampPersons, StampChanges, StampApprovals).
            ResultValueOkBind(signatures => InsertSignatures(signatures,
                                                             libraryElements.Select(libraryElement => libraryElement.Name).ToList())).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted =>
                    new ResultAppCollection<IStampSignature>(signaturesDeleted,
                                                             signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                  "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Вставить подписи и получить поля
        /// </summary>
        private IResultAppCollection<IStampSignature> InsertSignatures(IEnumerable<IStampSignature> signatures, IList<string> libraryIds) =>
            signatures.
            Select(signature => SignaturesSearching.
                                FindByIdOrFullNameOrRandom(signature.SignatureLibrary.PersonId,
                                                           signature.SignatureLibrary.PersonInformation.FullName, StampSettings.PersonId).
                                ResultValueContinue(signatureLibrary => libraryIds.IndexOf(signatureLibrary.PersonId) > 0,
                                    okFunc: signatureLibrary => signatureLibrary,
                                    badFunc: signatureLibrary => new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                      $"Подпись {signatureLibrary.PersonId} не найдена в библиотеке Microstation")).
                                ResultValueOk(signatureLibrary => new SignatureFileApp(signatureLibrary.PersonId,
                                                                                       signatureLibrary.PersonInformation, String.Empty)).
                                ResultValueOk(signature.InsertSignature)).
            ToResultCollection();
    }
}
