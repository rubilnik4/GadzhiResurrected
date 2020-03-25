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
        /// <summary>
        /// Функция вставки подписи
        /// </summary>
        private readonly Func<string, IStampFieldMicrostation> _insertSignatureFunc;

        public StampSignatureMicrostation(Func<string, IStampFieldMicrostation> insertSignatureFunc)
        {
            _insertSignatureFunc = insertSignatureFunc;
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldMicrostation Signature { get; protected set; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public ICellElementMicrostation SignatureElement => Signature.ElementStamp.AsCellElementMicrostation;

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature != null;

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature()
        {
            Signature = _insertSignatureFunc?.Invoke(PersonId);
            return this;
        }

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        public override void DeleteSignature()
        {
            if (IsSignatureValid)
            {
                Signature.ElementStamp.Remove();
                Signature = null;
            }
        }
    }
}
