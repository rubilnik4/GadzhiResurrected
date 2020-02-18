using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент типа Microstation
    /// </summary>
    public interface IElementMicrostation: IOwnerContainer
    {  
        /// <summary>
        /// Идентефикатор элемента
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым
        /// </summary>       
        bool IsTextElementMicrostation { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым полем
        /// </summary>       
        bool IsTextNodeElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовый элемент
        /// </summary>       
        ITextElementMicrostation AsTextElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовое поле
        /// </summary>       
        ITextNodeElementMicrostation AsTextNodeElementMicrostation { get; }
    }
}
