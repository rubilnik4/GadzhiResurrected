using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureWord : StampSignature<IStampFieldWord>, IStampSignatureWord
    {    
        public StampSignatureWord(IStampFieldWord signature,
                                  string signaturePath)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
            SignaturePath = signaturePath ?? throw new ArgumentNullException(nameof(signaturePath));
        }

        /// <summary>
        /// Путь файла подписи
        /// </summary>
        public string SignaturePath { get; }

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
            Signature.CellElementStamp.InsertPicture(SignaturePath);
            return this;
        }

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        public override void DeleteSignature()
        {
            if (IsSignatureValid)
            {
                Signature.CellElementStamp.DeleteAllPictures();
            }
        }
    }
}
