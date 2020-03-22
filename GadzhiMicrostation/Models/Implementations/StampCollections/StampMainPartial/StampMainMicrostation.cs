using GadzhiApplicationCommon.Models.Enums;
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
            StampPersonSignaturesMicrostation = GetStampPersonRowsWithoutSignatures();
            StampChangeSignaturesMicrostation = GetStampChangeRowsWithoutSignatures(StampPersonSignaturesMicrostation?.
                                                                                    FirstOrDefault().AttributePersonId);
            StampApprovalSignaturesMicrostation = GetStampApprovalRowsWithoutSignatures();
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IEnumerable<ICellElementMicrostation> InsertSignaturesFromLibrary()
        {
            StampPersonSignaturesMicrostation = StampPersonSignaturesMicrostation?.Select(person => GetStampPersonRowWithSignatures(person));

            StampChangeSignaturesMicrostation = StampChangeSignaturesMicrostation?.  //добавляем ID вставленной подписи, а не имени. Потому что подпись может не соответствовать имени
                                                Select(change => GetStampChangeRowWithSignatures(change, StampPersonSignaturesMicrostation.
                                                                                                         FirstOrDefault().SignatureElement.Name));

            StampApprovalSignaturesMicrostation = StampApprovalSignaturesMicrostation?.Select(approval => GetStampApprovalRowWithSignatures(approval));

            return StampPersonSignaturesMicrostation.Select(personSignature => personSignature.SignatureElement)?.
                   Union(StampChangeSignaturesMicrostation.Select(changeSignature => changeSignature.SignatureElement))?.
                   Union(StampApprovalSignaturesMicrostation.Select(approvalSignature => approvalSignature.SignatureElement));
                 
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
        public IEnumerable<IStampChangeSignature<IStampFieldMicrostation>> StampChangesSignatures =>
                StampChangeSignaturesMicrostation.Cast<IStampChangeSignature<IStampFieldMicrostation>>();

        /// <summary>
        /// Строки с согласованиями
        /// </summary>
        public IEnumerable<IStampApprovalSignatures<IStampFieldMicrostation>> StampApprovalSignatures =>
                StampApprovalSignaturesMicrostation.Cast<IStampApprovalSignatures<IStampFieldMicrostation>>();
      
        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override void DeleteSignatures()
        {
            foreach (var personSignature in StampPersonSignaturesMicrostation)
            {
                personSignature?.Signature.ElementStamp.Remove();
            }
        }
    }
}
