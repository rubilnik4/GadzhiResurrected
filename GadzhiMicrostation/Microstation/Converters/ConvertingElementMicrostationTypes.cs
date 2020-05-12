using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Converters
{
    /// <summary>
    /// Преобразовать тип элемента во внутренние типы Microstation
    /// </summary>
    public static class ConvertingElementMicrostationTypes
    {
        public static MsdElementType ToMsdMicrostation(ElementMicrostationType elementMicrostationType) =>
            elementMicrostationType switch
            {
                ElementMicrostationType.LineElement => MsdElementType.msdElementTypeLine,
                ElementMicrostationType.TextElement => MsdElementType.msdElementTypeText,
                ElementMicrostationType.TextNodeElement => MsdElementType.msdElementTypeTextNode,
                ElementMicrostationType.CellElement => MsdElementType.msdElementTypeCellHeader,
                _ => throw new InvalidEnumArgumentException(nameof(elementMicrostationType), (int)elementMicrostationType, typeof(ElementMicrostationType))
            };
    }
}
