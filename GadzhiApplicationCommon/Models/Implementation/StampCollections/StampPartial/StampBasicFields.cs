using System;
using System.Collections.Generic;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    using StampBasicFieldsFunc = Func<IStampTextField, IStampTextField, IStampBasicFields>;

    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        private IResultAppValue<IStampBasicFields> GetStampBasicFields() =>
            new ResultAppValue<StampBasicFieldsFunc>(GetStampBasicFieldsFunc()).
                ResultCurryOkBind(GetFullCode()).
                ResultCurryOkBind(GetCurrentSheet()).
                ResultValueOk(basicFieldsFunc => basicFieldsFunc.Invoke());

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected abstract IResultAppValue<IStampTextField> GetFullCode();

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        protected abstract IResultAppValue<IStampTextField> GetCurrentSheet();

        /// <summary>
        /// Функция создания класса с базовыми полями
        /// </summary>
        private static StampBasicFieldsFunc GetStampBasicFieldsFunc() =>
            (fullCode, currentSheet) => new StampBasicFields(fullCode, currentSheet);
    }
}