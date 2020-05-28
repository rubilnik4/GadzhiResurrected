using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.Fields;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;


namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    using StampBasicFieldsFunc = Func<IStampTextField, IStampTextField, IStampBasicFields>;

    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа
    /// </summary>
    public partial class StampMicrostation
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
        /// Функция создания класса с базовыми полями
        /// </summary>
        private static StampBasicFieldsFunc GetStampBasicFieldsFunc() =>
            (fullCode, currentSheet) => new StampBasicFields(fullCode, currentSheet);

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        private IResultAppValue<IStampTextField> GetFullCode() =>
                FindElementsInStamp<ITextElementMicrostation>(new List<string>() { StampFieldBasic.FullCode.Name },
                                                              new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра не найдено")).
                ResultValueOk(textElement => new StampTextFieldMicrostation(textElement.First(), StampFieldType.Basic));

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        private IResultAppValue<IStampTextField> GetCurrentSheet() =>
            FindElementsInStamp<ITextElementMicrostation>(new List<string>() { StampFieldBasic.CurrentSheet.Name },
                                                          new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле номера листа не найдено")).
            ResultValueOk(textElement => new StampTextFieldMicrostation(textElement.First(), StampFieldType.Basic));
    }
}