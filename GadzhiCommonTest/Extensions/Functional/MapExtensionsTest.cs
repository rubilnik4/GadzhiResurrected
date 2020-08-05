using System;
using System.Text;
using System.Collections.Generic;
using GadzhiCommon.Extensions.Functional;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GadzhiCommonTest.Extensions.Functional
{
    /// <summary>
    /// Проверка методов расширения для преобразования типов
    /// </summary>
    [TestClass]
    public class MapExtensionsTest
    {
        /// <summary>
        /// Проверка преобразование типов с помощью функции. Из числа в строку
        /// </summary>
        [TestMethod]
        public void Map_IntToString()
        {
            const int number = 2;

            string stringFromNumber = number.Map(numberConverting => numberConverting.ToString());

            Assert.AreEqual(stringFromNumber, "2");
        }

        /// <summary>
        /// Проверка преобразование типов с помощью функции. Число и строка в строку
        /// </summary>
        [TestMethod]
        public void Map_IntStringToString()
        {
            const int numberInt = 2;
            const string numberDouble = "2";

            string stringFromNumber = numberInt.Map(numberDouble, 
                                                    (numberFirst, numberSecond )=> numberFirst + numberSecond);

            Assert.AreEqual(stringFromNumber, "22");
        }

        /// <summary>
        /// Проверка выполнения действия
        /// </summary>
        [TestMethod]
        public void Void_CounterAddOne()
        {
            string testObject = "testObject";
            int counter = 0;
            void ActionCounter() => counter += 1;

            string testAfterVoid = testObject.Void(_ => ActionCounter());

            Assert.AreEqual(testAfterVoid, testObject);
            Assert.AreEqual(counter, 1);
        }
    }
}
