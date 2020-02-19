using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
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
        public static string GetAttributeById(Element element, ElementMicrostationAttributes attributeId)
        {
            string attributeName = String.Empty;

            var dataBlocks = element?.GetUserAttributeData((int)ElementMicrostationAttributes.AttributesArray).
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

            return GetNameInCorrectCase(attributeName);
        }

        /// <summary>
        /// Получить размеры ячейки элемента в стандартных координатах
        /// </summary>
        public static RangeMicrostation GetAttributeRange(Element element, bool isVertical)
        {
            string rangeInString = GetAttributeById(element, ElementMicrostationAttributes.Range);
            IList<string> rangeListInString = StampAdditionalParameters.SeparateAttributeValue(rangeInString);

            return new RangeMicrostation(rangeListInString, isVertical);
        }

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>
        public static string GetAttributePersonId(Element element) =>
             GetAttributeById(element, ElementMicrostationAttributes.PersonId).
             Trim('{', '}');

        /// <summary>
        /// Получить имя поля в корректном написании
        /// </summary>
        private static string GetNameInCorrectCase(string field) => field?.Trim()?.ToUpper();
    }
}
