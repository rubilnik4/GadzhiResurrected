using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GadzhiCommon.Extensions.Collection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GadzhiCommonTest.Extensions.Collection
{
    /// <summary>
    /// Тесты методов расширения для перечислений
    /// </summary>
    [TestClass]
    public class EnumerableExtensionTest
    {
        /// <summary>
        /// Проверка перечисления на null. Возвращает тот же объект
        /// </summary>  
        [TestMethod]
        public void EmptyIfNull_ReturnInitial()
        {
            var testCollection = new List<int>() { 3, 4, 5 };

            Assert.AreEqual(testCollection.EmptyIfNull(), testCollection);
        }

        /// <summary>
        /// Проверка перечисления на null. Возвращает пустой список
        /// </summary>  
        [TestMethod]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void EmptyIfNull_ReturnEmpty()
        {
            List<int> testCollection = null;

            Assert.IsTrue(testCollection.EmptyIfNull().SequenceEqual(Enumerable.Empty<int>()));
        }

        /// <summary>
        /// Проверка объединения элементов. Возвращает объединеннный список
        /// </summary>  
        [TestMethod]
        public void UnionNotNull_ReturnUnion()
        {
            var testFirstCollection = new List<int>() { 3, 4, 5 };
            var testSecondCollection = new List<int>() { 1, 2 };

            var unionResult = testFirstCollection.UnionNotNull(testSecondCollection);
            var unionExpected = testFirstCollection.Union(testSecondCollection);

            Assert.IsTrue(unionResult.SequenceEqual(unionExpected));
        }

        /// <summary>
        /// Проверка объединения элементов. Возвращает первый список
        /// </summary>  
        [TestMethod]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void UnionNotNull_ReturnFirst()
        {
            var testFirstCollection = new List<int>() { 3, 4, 5 };
            List<int> testSecondCollection = null;

            var unionResult = testFirstCollection.UnionNotNull(testSecondCollection);

            Assert.IsTrue(unionResult.SequenceEqual(testFirstCollection));
        }

        /// <summary>
        /// Проверка объединения элементов. Возвращает второй список
        /// </summary>  
        [TestMethod]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void UnionNotNull_ReturnSecond()
        {
            List<int> testFirstCollection = null;
            var testSecondCollection = new List<int>() { 1, 2 };

            var unionResult = testFirstCollection.UnionNotNull(testSecondCollection);

            Assert.IsTrue(unionResult.SequenceEqual(testSecondCollection));
        }

        /// <summary>
        /// Проверка объединения элементов. Возвращает пустой список
        /// </summary>  
        [TestMethod]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void UnionNotNull_ReturnEmpty()
        {
            List<int> testFirstCollection = null;
            List<int> testSecondCollection = null;

            var unionResult = testFirstCollection.UnionNotNull(testSecondCollection);

            Assert.IsTrue(unionResult.SequenceEqual(Enumerable.Empty<int>()));
        }
    }
}
