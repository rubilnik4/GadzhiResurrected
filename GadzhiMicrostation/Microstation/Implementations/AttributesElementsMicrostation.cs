using GadzhiMicrostation.Models.Enum;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Обработка аттрибутов элементов Microstation
    /// </summary>
    public static class AttributesElementsMicrostation
    {
        /// <summary>
        /// Получить значение аттрибута через его ID номер
        /// </summary>       
        public static string GetAttributeById(Element element, ElementAttributes attributeId)
        {
            string attributeName = String.Empty;

            var dataBlocks = element?.GetUserAttributeData((int)ElementAttributes.AttributesArray).
                                      Cast<DataBlock>();
          
            foreach (var datablock in dataBlocks)
            {               
                string attributeNameFromDataBlock = "";
                short attributeIdFromDataBlock = 0;

                datablock.CopyString(ref attributeNameFromDataBlock, false);
                datablock.CopyInteger(ref attributeIdFromDataBlock, false);

                if (attributeIdFromDataBlock == (int)attributeId)
                {
                    attributeName = attributeNameFromDataBlock;
                    break;
                }
            }

            return attributeName;
        }
    }
}
