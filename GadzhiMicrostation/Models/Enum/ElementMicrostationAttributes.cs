using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Enum
{
    /// <summary>
    /// Списки аттрибутов для элементов Microstation
    /// </summary>
    public enum ElementMicrostationAttributes
    {
        AttributesArray = 33452, //Массив с аттрибутами
        Range = 3401, //Размеры элемента в стандартных единицах
        ControlName = 3426, //Имя элемента
        PersonId = 3427, // идентефикатор подписанта
        Signature = 3428, // маркер элемента подписи
    }
}
