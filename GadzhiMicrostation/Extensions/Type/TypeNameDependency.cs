using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Helpers
{
    /// <summary>
    /// Преобразование типа в имя конструктора для инверсии зависимости
    /// </summary>
    public static class TypeNameDependency
    {
        /// <summary>
        /// Получить значение аттрибута по его Id номеру
        /// </summary>       
        public static string GetAttributeById(this Type type)
        {
            string controlName = AttributesElementsMicrostation.GetAttributeById(element, elementAttribute);
            return GetNameInCorrectCase(controlName);
        }
    }
}
