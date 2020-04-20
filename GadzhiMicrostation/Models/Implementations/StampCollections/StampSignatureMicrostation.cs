﻿using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
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
        private readonly Func<string, IResultAppValue<IStampFieldMicrostation>> _insertSignatureFunc;

        public StampSignatureMicrostation(Func<string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
        {
            _insertSignatureFunc = insertSignatureFunc;
            SignatureInitialize();
        }

        private void SignatureInitialize()
        {
            Signature = new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подпись не инициализирована").
                        ToResultApplicationValue<IStampFieldMicrostation>();
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampFieldMicrostation> Signature { get; protected set; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public IResultAppValue<ICellElementMicrostation> SignatureElement =>
            Signature.ResultValueOk(signature => signature.ElementStamp.AsCellElementMicrostation);

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature.OkStatus;

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
        public override void DeleteSignature() =>
            Signature.
            ResultVoidOk(signature =>
            {
                Signature.Value.ElementStamp.Remove();
                SignatureInitialize();
            });
    }
}
