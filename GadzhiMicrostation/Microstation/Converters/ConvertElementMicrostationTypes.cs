using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Converters
{
    /// <summary>
    /// Преобразовать тип элемента во внутренние типы Microstation
    /// </summary>
    public static class ConvertElementMicrostationTypes
    {
        public static MsdElementType? ConvertToMsdMicrostation(ElementMicrostationType elementMicrostationType)
        {
            MsdElementType? msdElementType = null;

            if (elementMicrostationType == ElementMicrostationType.TextElement)
            {
                msdElementType = MsdElementType.msdElementTypeText;
            }
            else if (elementMicrostationType == ElementMicrostationType.TextNodeElement)
            {
                msdElementType = MsdElementType.msdElementTypeTextNode;
            }
            else if (elementMicrostationType == ElementMicrostationType.CellElement)
            {
                msdElementType = MsdElementType.msdElementTypeCellHeader;
            }

            return msdElementType;
        }
    }
}
