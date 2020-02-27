using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Фрагмент библиотеки Microstation
    /// </summary>
    public class LibraryElement
    {
        public LibraryElement(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Имя фрагмента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
