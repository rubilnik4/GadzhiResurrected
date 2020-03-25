using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
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
        /// <summary>
        /// Функция вставки подписи
        /// </summary>
        private readonly string _signaturePath;

        public StampSignatureWord(IStampFieldWord signature,
                                  string signaturePath)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));

            _signaturePath =  String.IsNullOrEmpty(signaturePath) ?
                              signaturePath :
                              throw new ArgumentNullException(nameof(signaturePath));
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldWord Signature { get; protected set; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature?.CellElementStamp.HasPicture == true;

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldWord> InsertSignature()
        {
            Signature.CellElementStamp.InsertPicture(_signaturePath);
            return this;
        }

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        public override void DeleteSignature()
        {
            if (IsSignatureValid)
            {
                Signature.CellElementStamp.DeleteAllPictures ();               
            }
        }
    }
}
