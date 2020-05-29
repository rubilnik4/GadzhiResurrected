using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Collection;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;

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
        protected override IResultAppValue<IStampTextField> GetFullCode() =>
            new ResultAppValue<IStampTextField>(GetFieldsByType(StampFieldType.FullRow).FirstOrDefault(),
                                                new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра в таблице не найдено"));

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        protected override IResultAppValue<IStampTextField> GetCurrentSheet() =>
            new ResultAppValue<IStampTextField>(GetFieldsByType(StampFieldType.CurrentSheet).FirstOrDefault(),
                                                new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле текущего листа в таблице не найдено"));
    }
}