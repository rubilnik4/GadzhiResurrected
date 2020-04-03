using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public partial class StampMainMicrostation : StampMicrostation, IStampMain<IStampFieldMicrostation>
    {

        /// <summary>
        /// Строки с ответсвенным лицом и подписью Microstation
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> StampPersonSignaturesMicrostation { get; set; }

        /// <summary>
        /// Строки с изменениями Microstation
        /// </summary>
        private IEnumerable<IStampChangeSignatureMicrostation> StampChangeSignaturesMicrostation { get; set; }

        /// <summary>
        /// Строки с согласованиями Microstation
        /// </summary>
        private IEnumerable<IStampApprovalSignatureMicrostation> StampApprovalSignaturesMicrostation { get; set; }

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
        protected override IEnumerable<IErrorApplication> InsertSignaturesFromLibrary() =>
            GetSignatures(StampPersonSignaturesMicrostation, StampChangeSignaturesMicrostation, StampApprovalSignaturesMicrostation).
            Select(signature => signature.InsertSignature()).
            Where(signature => !signature.IsSignatureValid).
            Select(signature => new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                     $"Не найдена подпись {signature.PersonName}")).
            Cast<IErrorApplication>();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override void DeleteSignatures()
        {
            var signaturesToDelete = GetSignatures(StampPersonSignaturesMicrostation,
                                                   StampChangeSignaturesMicrostation,
                                                   StampApprovalSignaturesMicrostation).
                                     Where(signature => signature.IsSignatureValid);

            foreach (var signature in signaturesToDelete)
            {
                signature?.DeleteSignature();
            }
        }

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
        private IEnumerable<IStampSignature<IStampFieldMicrostation>> GetSignatures(IEnumerable<IStampPersonSignatureMicrostation> personSignatures,
                                                                                    IEnumerable<IStampChangeSignatureMicrostation> changeSignatures,
                                                                                    IEnumerable<IStampApprovalSignatureMicrostation> approvalSignatures) =>
             ((IEnumerable<IStampSignature<IStampFieldMicrostation>>)personSignatures).
             Union((IEnumerable<IStampSignature<IStampFieldMicrostation>>)changeSignatures).
             Union((IEnumerable<IStampSignature<IStampFieldMicrostation>>)approvalSignatures);
    }
}
