using System;

namespace GadzhiMicrostation.Models.Enums
{
    /// <summary>
    /// Списки аттрибутов для элементов Microstation
    /// </summary>
    [Flags]
    public enum ElementMicrostationAttributes
    {
        AttributesArray = 33452, //Массив с аттрибутами
        Range = 3401, //Размеры элемента в стандартных единицах
        ControlName = 3426, //Имя элемента
        PersonId = 3427, // идентефикатор подписанта
        Signature = 3428, // маркер элемента подписи
    }
}
