using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

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
            string attributeValue = String.Empty;
            var dataBlocks = element?.GetUserAttributeData((int)ElementMicrostationAttributes.AttributesArray).
                                      Cast<DataBlock>().
                                      ToList();
            //поиск с обратного конца из-за возможных накоплений атрибутов
            if (dataBlocks != null)
            {
                for (int i = dataBlocks.Count - 1; i >= 0; i--)
                {
                    string attributeValueFromDataBlock = "";
                    short attributeIdFromDataBlock = 0;

                    DataBlockReadWriteOperation(dataBlocks[i], ref attributeValueFromDataBlock, ref attributeIdFromDataBlock, DataBlockOperationType.Read);

                    if (attributeIdFromDataBlock == (int)attributeId)
                    {
                        attributeValue = attributeValueFromDataBlock;
                        break;
                    }
                }
            }

            return GetAttributeNameInCorrectCase(attributeValue);
        }

        /// <summary>
        /// Записать значение аттрибута через его ID номер
        /// </summary>       
        public static void SetAttributeById(Element element,
                                            ElementMicrostationAttributes attributeId,
                                            string attributeValue)
        {
            if (!String.IsNullOrEmpty(attributeValue))
            {
                attributeValue = GetAttributeNameInCorrectCase(attributeValue);

                bool isAttributeFound = DataBlockCheckAttributeAndWriteIfExist(element, attributeId, attributeValue);

                if (!isAttributeFound)
                {
                    DataBlockAddNewAttribute(element, attributeId, attributeValue);
                }
            }
        }

        /// <summary>
        /// Получить размеры ячейки элемента в стандартных координатах
        /// </summary>
        public static IResultValue<RangeMicrostation> GetAttributeRange(Element element) =>
           GetAttributeById(element, ElementMicrostationAttributes.Range).
           Map(rangeInlineString => RangeMicrostation.StringToRange(rangeInlineString));

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>
        public static string GetAttributeControlName(Element element) =>
             GetAttributeById(element, ElementMicrostationAttributes.ControlName);

        /// <summary>
        /// Записать идентефикатор личности
        /// </summary>
        public static void SetAttributeControlName(Element element, string controlName) =>
             SetAttributeById(element, ElementMicrostationAttributes.ControlName, controlName);

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>
        public static string GetAttributePersonId(Element element) =>
             GetAttributeById(element, ElementMicrostationAttributes.PersonId).
             Trim('{', '}');

        /// <summary>
        /// Получить имя поля в корректном написании
        /// </summary>
        private static string GetAttributeNameInCorrectCase(string field) => field?.Trim()?.ToUpper(CultureInfo.CurrentCulture);

        /// <summary>
        /// Прочитать/записать данные в блок
        /// </summary>      
        private static void DataBlockReadWriteOperation(DataBlock dataBlock,
                                                        ref string attributeValueFromDataBlock,
                                                        ref short attributeIdFromDataBlock,
                                                        DataBlockOperationType dataBlockOperationType)
        {
            dataBlock.CopyString(ref attributeValueFromDataBlock, Convert.ToBoolean(dataBlockOperationType, CultureInfo.CurrentCulture));
            dataBlock.CopyInteger(ref attributeIdFromDataBlock, Convert.ToBoolean(dataBlockOperationType, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Проверить блок и записать аттрибут при наличии
        /// </summary>
        private static bool DataBlockCheckAttributeAndWriteIfExist(Element element,
                                                                   ElementMicrostationAttributes attributeId,
                                                                   string attributeValue)
        {
            bool isAttributeFound = false;
            var dataBlocks = element?.GetUserAttributeData((int)ElementMicrostationAttributes.AttributesArray).
                                      Cast<DataBlock>().
                                      ToList();

            for (int i = dataBlocks.Count - 1; i >= 0; i--)
            {
                string attributeValueFromDataBlock = "";
                short attributeIdFromDataBlock = 0;

                DataBlockReadWriteOperation(dataBlocks[i], ref attributeValueFromDataBlock, ref attributeIdFromDataBlock, DataBlockOperationType.Read);

                if (attributeIdFromDataBlock == (short)attributeId &&
                    attributeValueFromDataBlock != attributeValue)
                {
                    //аттрибуты не удаляются, а копятся. Не самое лучшее решение. Избегать установки аттрибутов                    
                    element.DeleteUserAttributeData(attributeIdFromDataBlock, (short)i);
                    DataBlockAddNewAttribute(element, attributeId, attributeValue);

                    isAttributeFound = true;
                }
            }

            return isAttributeFound;
        }

        private static void DataBlockAddNewAttribute(Element element,
                                                     ElementMicrostationAttributes attributeId,
                                                     string attributeValue)
        {
            DataBlock dataBlock = new DataBlock();
            short attributeIdToDataBlock = (short)attributeId;

            DataBlockReadWriteOperation(dataBlock, ref attributeValue, ref attributeIdToDataBlock, DataBlockOperationType.Write);

            element?.AddUserAttributeData((int)ElementMicrostationAttributes.AttributesArray, dataBlock);
        }
    }
}
