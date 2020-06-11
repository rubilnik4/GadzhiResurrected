using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.Fields;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.BasicFieldsCreatingPartial
{
    /// <summary>
    /// Фабрика создания базовых полей Microstation
    /// </summary>
    public class BasicFieldsCreatingMicrostation : BasicFieldsCreating
    {
        public BasicFieldsCreatingMicrostation(IStampFieldsMicrostation stampFieldsMicrostation)
        {
            _stampFields = stampFieldsMicrostation ?? throw new ArgumentNullException(nameof(stampFieldsMicrostation));
        }

        /// <summary>
        /// Поля штампа Microstation
        /// </summary>
        private readonly IStampFieldsMicrostation _stampFields;

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        public override IResultAppValue<IStampTextField> GetFullCode() =>
                _stampFields.FindElementsInStamp<ITextNodeElementMicrostation>(new List<string>() { StampFieldBasic.FullCode.Name },
                                                                  new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра не найдено")).
                ResultValueOk(textElement => new StampTextNodeFieldMicrostation(textElement.First(), StampFieldType.FullRow));

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        public override IResultAppValue<IStampTextField> GetCurrentSheet() =>
            _stampFields.FindElementsInStamp<ITextElementMicrostation>(new List<string>() { StampFieldBasic.CurrentSheet.Name },
                                                          new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле номера листа не найдено")).
            ResultValueOk(textElement => new StampTextFieldMicrostation(textElement.First(), StampFieldType.CurrentSheet));
    }
}