using System;
using GadzhiApplicationCommon.Helpers;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Fields
{
    public class StampTextNodeFieldMicrostation : StampFieldMicrostation, IStampTextFieldMicrostation
    {

        public StampTextNodeFieldMicrostation(ITextNodeElementMicrostation textElementStamp, StampFieldType stampFieldType)
            :base(textElementStamp, stampFieldType)
        {
            TextNodeElementStamp = textElementStamp ?? throw new ArgumentNullException(nameof(textElementStamp));
        }

        /// <summary>
        /// Текстовый элемент, определяющий поле штампа
        /// </summary>
        public ITextNodeElementMicrostation TextNodeElementStamp { get; }

        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public string Text => TextNodeElementStamp.TextInline;

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        public string MaxLengthWord => TextFormatting.GetMaxLengthWord(Text);
    }
}