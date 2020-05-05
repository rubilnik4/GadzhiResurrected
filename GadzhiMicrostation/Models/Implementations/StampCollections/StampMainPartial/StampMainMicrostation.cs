using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

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
        private IEnumerable<IStampPersonSignatureMicrostation> StampPersonSignaturesMicrostation { get;  }

        /// <summary>
        /// Строки с изменениями Microstation
        /// </summary>
        private IEnumerable<IStampChangeSignatureMicrostation> StampChangeSignaturesMicrostation { get; }

        /// <summary>
        /// Строки с согласованиями Microstation
        /// </summary>
        private IEnumerable<IStampApprovalSignatureMicrostation> StampApprovalSignaturesMicrostation { get;  }

        public StampMainMicrostation(ICellElementMicrostation stampCellElement)
            : base(stampCellElement)
        {
            StampPersonSignaturesMicrostation = GetStampPersonRowsWithoutSignatures().ToList();

            var firstPerson = StampPersonSignaturesMicrostation?.FirstOrDefault();
            StampChangeSignaturesMicrostation = GetStampChangeRowsWithoutSignatures(firstPerson?.PersonId, firstPerson?.PersonName).ToList();

            StampApprovalSignaturesMicrostation = GetStampApprovalRowsWithoutSignatures().ToList();
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IResultAppCollection<IStampSignature<IStampField>> InsertSignaturesFromLibrary() =>
            GetSignatures(StampPersonSignaturesMicrostation, StampChangeSignaturesMicrostation,StampApprovalSignaturesMicrostation).
            Select(signature => signature.InsertSignature()).
            Cast<IStampSignature<IStampField>>().
            ToList().
            Map(signaturesDeleted => new ResultAppCollection<IStampSignature<IStampField>>
                             (signaturesDeleted,
                              signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                              new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для вставки не инициализированы")));

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
        public IEnumerable<IStampPersonSignature<IStampFieldMicrostation>> StampPersonSignatures =>
                StampPersonSignaturesMicrostation.Cast<IStampPersonSignature<IStampFieldMicrostation>>();

        /// <summary>
        /// Строки с изменениями
        /// </summary>
        public IEnumerable<IStampChangeSignature<IStampFieldMicrostation>> StampChangeSignatures =>
                StampChangeSignaturesMicrostation.Cast<IStampChangeSignature<IStampFieldMicrostation>>();

        /// <summary>
        /// Строки с согласованиями
        /// </summary>
        public IEnumerable<IStampApprovalSignatures<IStampFieldMicrostation>> StampApprovalSignatures =>
                StampApprovalSignaturesMicrostation.Cast<IStampApprovalSignatures<IStampFieldMicrostation>>();

        /// <summary>
        /// Получить подписи
        /// </summary>        
        private static IEnumerable<IStampSignature<IStampFieldMicrostation>> GetSignatures(IEnumerable<IStampPersonSignatureMicrostation> personSignatures,
                                                                                           IEnumerable<IStampChangeSignatureMicrostation> changeSignatures,
                                                                                           IEnumerable<IStampApprovalSignatureMicrostation> approvalSignatures) =>
             ((IEnumerable<IStampSignature<IStampFieldMicrostation>>)personSignatures).
             Union((IEnumerable<IStampSignature<IStampFieldMicrostation>>)changeSignatures).
             Union((IEnumerable<IStampSignature<IStampFieldMicrostation>>)approvalSignatures);
    }
}
