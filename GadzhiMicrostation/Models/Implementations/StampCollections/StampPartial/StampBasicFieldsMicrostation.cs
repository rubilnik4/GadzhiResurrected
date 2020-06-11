using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.Fields;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;


namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа Microstation
    /// </summary>
    public partial class StampMicrostation
    {
        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected override IResultAppValue<IStampTextField> GetFullCode =>
                FindElementsInStamp<ITextNodeElementMicrostation>(new List<string>() { StampFieldBasic.FullCode.Name },
                                                                  new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра не найдено")).
                ResultValueOk(textElement => new StampTextNodeFieldMicrostation(textElement.First(), StampFieldType.FullRow));

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        protected override IResultAppValue<IStampTextField> CurrentSheet =>
            FindElementsInStamp<ITextElementMicrostation>(new List<string>() { StampFieldBasic.CurrentSheet.Name },
                                                          new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле номера листа не найдено")).
            ResultValueOk(textElement => new StampTextFieldMicrostation(textElement.First(), StampFieldType.CurrentSheet));
    }
}