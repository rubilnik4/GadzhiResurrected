using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiCommonTest.Extensions.Functional
{
    /// <summary>
    /// Тесты методов расширения для функций высшего порядка
    /// </summary>
    [TestClass]
    public class CurryExtensionsTest
    {
        /// <summary>
        /// Проверка преобразования функции высшего порядка для одного аргумента
        /// </summary>
        [TestMethod]
        public void Curry_ReturnNoArgumentFunc()
        {
            Func<int, int> plusTwoFunc = number => number + 2;

            var totalFunc = plusTwoFunc.Curry(3);

            Assert.AreEqual(totalFunc.Invoke(), 5);
        }

        /// <summary>
        /// Проверка преобразования функции высшего порядка для двух аргументов
        /// </summary>
        [TestMethod]
        public void Curry_ReturnOneArgumentFunc()
        {
            Func<int, int, int> plusTwoFunc = (numberFirst, numberSecond) => numberFirst + numberSecond;

            var totalFunc = plusTwoFunc.Curry(3);
            
            Assert.AreEqual(totalFunc.Invoke(2), 5);
        }

        /// <summary>
        /// Проверка преобразования функции высшего порядка для трех аргументов
        /// </summary>
        [TestMethod]
        public void Curry_ReturnTwoArgumentFunc()
        {
            Func<int, int, int, int> plusTwoFunc = (numberFirst, numberSecond, thirdNumber) => numberFirst + numberSecond + thirdNumber;

            var totalFunc = plusTwoFunc.Curry(3);

            Assert.AreEqual(totalFunc.Invoke(2, 1), 6);
        }
    }
}
