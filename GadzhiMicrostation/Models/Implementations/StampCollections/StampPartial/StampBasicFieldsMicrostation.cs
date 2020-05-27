using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа
    /// </summary>
    public partial class StampMicrostation
    {
        private IResultAppValue<IStampBasicFields> GetStampBasicFields()
        {
            var fullCode = FindElementsInStamp<ITextElementMicrostation>(new List<string>() { StampFieldBasic.FullCode.Name },
                                                                         new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра не найдено");


        }
    }
}