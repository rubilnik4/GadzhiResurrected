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
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public partial class StampMainMicrostation : StampMicrostation, IStampMain<IStampFieldMicrostation>
    {
        /// <summary>
        /// Строки с ответственным лицом и подписью Microstation
        /// </summary>
        private IResultAppCollection<IStampPersonMicrostation> StampPersonsMicrostation { get; }

        /// <summary>
        /// Строки с изменениями Microstation
        /// </summary>
        private IResultAppCollection<IStampChangeMicrostation> StampChangesMicrostation { get; }

        /// <summary>
        /// Строки с согласованиями Microstation
        /// </summary>
        private IResultAppCollection<IStampApprovalMicrostation> StampApprovalsMicrostation { get; }

        public StampMainMicrostation(ICellElementMicrostation stampCellElement, StampIdentifier id)
            : base(stampCellElement, id)
        {
            StampPersonsMicrostation = GetStampPersonRows();

            var firstPerson = StampPersonsMicrostation.Value?.FirstOrDefault();
            StampChangesMicrostation = GetStampChangeRows(firstPerson?.PersonId, firstPerson?.PersonName);

            StampApprovalsMicrostation = GetStampApprovalRows();
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IResultAppCollection<IStampSignature<IStampField>> InsertSignaturesFromLibrary(IList<LibraryElement> libraryElements) =>
            GetSignatures(StampPersonsMicrostation, StampChangesMicrostation, StampApprovalsMicrostation).
            ResultValueOk(signatures => signatures.
                                        Select(signature => signature.InsertSignature(libraryElements)).
                                        Cast<IStampSignature<IStampField>>().
                                        ToList()).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature<IStampField>> DeleteSignatures(IEnumerable<IStampSignature<IStampField>> signatures) =>
            signatures.
            Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted => new ResultAppCollection<IStampSignature<IStampField>>
                                     (signaturesDeleted,
                                      signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                                      new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        public IResultAppCollection<IStampPerson<IStampFieldMicrostation>> StampPersons =>
            StampPersonsMicrostation.Cast<IStampPersonMicrostation, IStampPerson<IStampFieldMicrostation>>();

        /// <summary>
        /// Строки с изменениями
        /// </summary>
        public IResultAppCollection<IStampChange<IStampFieldMicrostation>> StampChanges =>
            StampChangesMicrostation.Cast<IStampChangeMicrostation, IStampChange<IStampFieldMicrostation>>();

        /// <summary>
        /// Строки с согласованиями
        /// </summary>
        public IResultAppCollection<IStampApproval<IStampFieldMicrostation>> StampApprovals =>
            StampApprovalsMicrostation.Cast<IStampApprovalMicrostation, IStampApproval<IStampFieldMicrostation>>();

        /// <summary>
        /// Получить подписи
        /// </summary>        
        private static IResultAppCollection<IStampSignatureMicrostation> GetSignatures(IResultAppCollection<IStampPersonMicrostation> personSignatures,
                                                                                       IResultAppCollection<IStampChangeMicrostation> changeSignatures,
                                                                                       IResultAppCollection<IStampApprovalMicrostation> approvalSignatures) =>
             personSignatures.Cast<IStampPersonMicrostation, IStampSignatureMicrostation>().
                              ConcatValues(changeSignatures.Value.Cast<IStampSignatureMicrostation>()).
                              ConcatValues(approvalSignatures.Value.Cast<IStampSignatureMicrostation>());
    }
}
