﻿using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Ошибка приложения конвертации
    /// </summary>
    public class ErrorApplication : IErrorApplication
    {
        public ErrorApplication(ErrorApplicationType errorMicrostationType, string errorDescription)
        {
            ErrorMicrostationType = errorMicrostationType;
            Description = errorDescription ?? String.Empty;
        }

        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        public ErrorApplicationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Преобразовать в ответ
        /// </summary>      
        public IResultApplication ToResultApplication() => new ResultApplication(this);

        /// <summary>
        /// Преобразовать в ответ с вложенным типом
        /// </summary>      
        public IResultAppValue<TValue> ToResultApplicationValue<TValue>() => new ResultAppValue<TValue>(this);

        /// <summary>
        /// Преобразовать в ответ с коллекцией
        /// </summary>      
        public IResultAppCollection<TValue> ToResultCollection<TValue>() => new ResultAppCollection<TValue>(this);

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
