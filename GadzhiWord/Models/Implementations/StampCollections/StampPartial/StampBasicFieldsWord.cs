using System;
using System.Collections.Generic;

using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected override IResultAppValue<IStampTextField> GetFullCode() => throw new NotImplementedException(); 

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        protected override IResultAppValue<IStampTextField> GetCurrentSheet() => throw new NotImplementedException();
    }
}