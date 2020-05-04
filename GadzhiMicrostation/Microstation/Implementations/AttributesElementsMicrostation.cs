using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Обработка атрибутов элементов Microstation
    /// </summary>
    public static class AttributesElementsMicrostation
    {
        /// <summary>
        /// Получить значение атрибута через его ID номер
        /// </summary>       
        public static string GetAttributeById(Element element, ElementMicrostationAttributes attributeId)
        {
            var attributeValue = String.Empty;
            var dataBlocks = element?.GetUserAttributeData((int)ElementMicrostationAttributes.AttributesArray).
                                      Cast<DataBlock>().
                                      ToList();
            //поиск с обратного конца из-за возможных накоплений атрибутов
            if (dataBlocks == null) return GetAttributeNameInCorrectCase(attributeValue);

            for (int i = dataBlocks.Count - 1; i >= 0; i--)
            {
                var attributeValueFromDataBlock = String.Empty;
                short attributeIdFromDataBlock = 0;

                DataBlockReadWriteOperation(dataBlocks[i], ref attributeValueFromDataBlock, ref attributeIdFromDataBlock, DataBlockOperationType.Read);

                if (attributeIdFromDataBlock == (int)attributeId)
                {
                    attributeValue = attributeValueFromDataBlock;
                    break;
                }
            }

            return GetAttributeNameInCorrectCase(attributeValue);
        }

        /// <summary>
        /// Записать значение атрибута через его ID номер
        /// </summary>       
        public static void SetAttributeById(Element element, ElementMicrostationAttributes attributeId, string attributeValue)
        {
            if (String.IsNullOrEmpty(attributeValue)) return;

            attributeValue = GetAttributeNameInCorrectCase(attributeValue);

            bool isAttributeFound = DataBlockCheckAttributeAndWriteIfExist(element, attributeId, attributeValue);
            if (!isAttributeFound)
            {
                DataBlockAddNewAttribute(element, attributeId, attributeValue);
            }
        }

        /// <summary>
        /// Получить размеры ячейки элемента в стандартных координатах
        /// </summary>
        public static IResultAppValue<RangeMicrostation> GetAttributeRange(Element element) =>
           GetAttributeById(element, ElementMicrostationAttributes.Range).
           Map(RangeMicrostation.StringToRange);

        /// <summary>
        /// Получить идентификатор личности
        /// </summary>
        public static string GetAttributeControlName(Element element) =>
             GetAttributeById(element, ElementMicrostationAttributes.ControlName);

        /// <summary>
        /// Записать идентификатор личности
        /// </summary>
        public static void SetAttributeControlName(Element element, string controlName) =>
             SetAttributeById(element, ElementMicrostationAttributes.ControlName, controlName);

        /// <summary>
        /// Получить идентификатор личности
        /// </summary>
        public static string GetAttributePersonId(Element element) =>
             GetAttributeById(element, ElementMicrostationAttributes.PersonId).
             Trim('{', '}');

        /// <summary>
        /// Получить имя поля в корректном написании
        /// </summary>
        private static string GetAttributeNameInCorrectCase(string field) => field?.Trim().ToUpper(CultureInfo.CurrentCulture);

        /// <summary>
        /// Прочитать/записать данные в блок
        /// </summary>      
        private static void DataBlockReadWriteOperation(DataBlock dataBlock, ref string attributeValueFromDataBlock,
                                                        ref short attributeIdFromDataBlock, DataBlockOperationType dataBlockOperationType)
        {
            dataBlock.CopyString(ref attributeValueFromDataBlock, Convert.ToBoolean(dataBlockOperationType, CultureInfo.CurrentCulture));
            dataBlock.CopyInteger(ref attributeIdFromDataBlock, Convert.ToBoolean(dataBlockOperationType, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Проверить блок и записать атрибут при наличии
        /// </summary>
        private static bool DataBlockCheckAttributeAndWriteIfExist(Element element, ElementMicrostationAttributes attributeId,
                                                                   string attributeValue)
        {
            if (element == null) return false;

            var dataBlocks = element.GetUserAttributeData((int) ElementMicrostationAttributes.AttributesArray).
                             Cast<DataBlock>().
                             ToList();

            bool isAttributeFound = false;
            for (int i = dataBlocks.Count - 1; i >= 0; i--)
            {
                var attributeValueFromDataBlock = String.Empty;
                short attributeIdFromDataBlock = 0;

                DataBlockReadWriteOperation(dataBlocks[i], ref attributeValueFromDataBlock, ref attributeIdFromDataBlock, DataBlockOperationType.Read);

                if (attributeIdFromDataBlock == (short)attributeId &&
                    attributeValueFromDataBlock != attributeValue)
                {
                    //атрибуты не удаляются, а копятся. Не самое лучшее решение. Избегать установки атрибутов                    
                    element.DeleteUserAttributeData(attributeIdFromDataBlock, (short)i);
                    DataBlockAddNewAttribute(element, attributeId, attributeValue);

                    isAttributeFound = true;
                }
            }

            return isAttributeFound;
        }

        private static void DataBlockAddNewAttribute(Element element, ElementMicrostationAttributes attributeId, string attributeValue)
        {
            var dataBlock = new DataBlock();
            var attributeIdToDataBlock = (short)attributeId;

            DataBlockReadWriteOperation(dataBlock, ref attributeValue, ref attributeIdToDataBlock, DataBlockOperationType.Write);

            element?.AddUserAttributeData((int)ElementMicrostationAttributes.AttributesArray, dataBlock);
        }
    }
}
