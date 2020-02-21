using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public class TextElementMicrostation : RangeBaseElementMicrostation, ITextElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextElement _textElement;

        public TextElementMicrostation(TextElement textElement,
                                       IOwnerContainerMicrostation ownerContainerMicrostation)
            : this(textElement, ownerContainerMicrostation, true, false)
        {
        }

        public TextElementMicrostation(TextElement textElement,
                                       IOwnerContainerMicrostation ownerContainerMicrostation,
                                       bool isNeedCompress,
                                       bool isVertical)
            : base((Element)textElement, ownerContainerMicrostation, isNeedCompress, isVertical)
        {
            _textElement = textElement;
        }

        /// <summary>
        /// Текст элемента
        /// </summary>
        public string Text => _textElement?.Text;

        /// <summary>
        /// Координаты текстового элемента
        /// </summary>
        public override PointMicrostation Origin => _textElement.get_Origin().ToPointMicrostation();

        /// <summary>
        /// Вписать текстовый элемент в рамку
        /// </summary>
        public override bool CompressRange()
        {
            bool isComressed = false;

            if (IsNeedCompress == true)
            {
                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatioText < Width)
                {
                    double compressionLevel = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatioText;
                    _textElement.TextStyle.Width *= compressionLevel;
                    _textElement.Rewrite();

                    isComressed = true;
                }
            }

            return isComressed;
        }
    }
}
