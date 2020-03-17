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
        public StampMainMicrostation(ICellElementMicrostation stampCellElement)
            : base(stampCellElement)
        {
            StampPersonSignaturesMicrostation = GetStampPersonRowsWithoutSignatures();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью Microstation
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> StampPersonSignaturesMicrostation { get; set; }

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldMicrostation>> StampPersonSignatures =>
                StampPersonSignaturesMicrostation.Cast<IStampPersonSignature<IStampFieldMicrostation>>();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        protected override IEnumerable<ICellElementMicrostation> InsertSignaturesFromLibrary()
        {
            StampPersonSignaturesMicrostation = StampPersonSignaturesMicrostation.Select(person => GetStampPersonRowWithSignatures(person));
            return StampPersonSignaturesMicrostation.Select(personSignature => personSignature.SignatureElement);
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override void DeleteSignatures()
        {
            foreach (var personSignature in StampPersonSignatures)
            {
                personSignature?.Signature.ElementStamp.Remove();
            }
        }
    }
}
