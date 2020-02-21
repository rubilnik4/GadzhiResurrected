using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using System.Collections.Generic;

namespace GadzhiMicrostation.Microstation.Interfaces.StampPartial
{
    public interface IStampDataFields
    {
        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<string> fieldSearch,
                                                                    ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element);

        /// <summary>
        /// Найти элемент в словаре штампа по ключам
        /// </summary>
        IElementMicrostation FindElementInStampFields(string fieldSearch);

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        void CompressFieldsRanges();
    }
}
