using System;
using GadzhiApplicationCommon.Helpers;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Fields
{
    public class StampTextFieldMicrostation: StampFieldMicrostation, IStampTextFieldMicrostation
    {

        public StampTextFieldMicrostation(ITextElementMicrostation textElementStamp, StampFieldType stampFieldType)
            :base(textElementStamp, stampFieldType)
        {
            TextElementStamp = textElementStamp ?? throw new ArgumentNullException(nameof(textElementStamp));
        }

        /// <summary>
        /// Текстовый элемент, определяющий поле штампа
        /// </summary>
        public ITextElementMicrostation TextElementStamp { get; }

        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public string Text => TextElementStamp.Text;

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        public string MaxLengthWord => TextFormatting.GetMaxLengthWord(Text);
    }
}