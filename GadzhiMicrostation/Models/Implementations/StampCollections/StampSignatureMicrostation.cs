using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureMicrostation : StampSignature<IStampFieldMicrostation>
    {
        public StampSignatureMicrostation() { }

        public StampSignatureMicrostation(IStampFieldMicrostation signature)
        {
            Signature = signature;
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldMicrostation Signature { get; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public ICellElementMicrostation SignatureElement => Signature.ElementStamp.AsCellElementMicrostation;

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature != null;
    }
}
