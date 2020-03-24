using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiWord.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureWord : StampSignature<IStampFieldWord>
    {
        public StampSignatureWord(IStampFieldWord signature)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldWord Signature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary> 
        public override string AttributePersonId => throw new NotImplementedException();

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature?.CellElementStamp.HasPicture == true;
    }
}
