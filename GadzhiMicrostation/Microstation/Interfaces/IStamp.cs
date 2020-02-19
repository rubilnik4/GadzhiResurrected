using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Штамп
    /// </summary>
    public interface IStamp
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

        /// <summary>
        /// Вставить подписи
        /// </summary>
        void InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        void DeleteSignatures();   
    }
}
