using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation
{
    /// <summary>
    /// Ошибка приложения конвертации
    /// </summary>
    public class ErrorApplication: IErrorApplication
    {
        public ErrorApplication(ErrorApplicationType errorMicrostationType, string errorDescription)
        {
            ErrorMicrostationType = errorMicrostationType;
            ErrorDescription = errorDescription ?? string.Empty;
        }

        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        public ErrorApplicationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string ErrorDescription { get; }

        /// <summary>
        /// Реализация перечисления
        /// </summary>      
        public IEnumerator<IErrorApplication> GetEnumerator()
        {
            yield return this;
        }

        /// <summary>
        /// Реализация перечисления
        /// </summary>     
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();   
    }
}
