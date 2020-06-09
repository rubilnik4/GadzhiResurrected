using System;
using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected abstract IResultAppValue<IStampTextField> FullCode { get; }

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        protected abstract IResultAppValue<IStampTextField> CurrentSheet { get; }

        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        private IStampBasicFields GetStampBasicFields() => new StampBasicFields(FullCode, CurrentSheet);
    }
}