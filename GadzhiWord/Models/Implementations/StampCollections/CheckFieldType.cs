using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Класс для определения типа поля
    /// </summary>
    public static class CheckFieldType
    {
        /// <summary>
        /// Опередлить тип поля штампа
        /// </summary>
        public static StampFieldType GetStampFieldType(string cellText)
        {
            StampFieldType stampFieldType = StampFieldType.Unknown;

            cellText = StringAdditionalExtensions.PrepareCellTextToComprare(cellText);
            if (IsFieldPersonSignature(cellText))
            {
                stampFieldType = StampFieldType.PersonSignature;
            }

            return stampFieldType;
        }

        /// <summary>
        /// Находится ли поле в строке с ответственным лицом и подписью
        /// </summary>        
        public static bool IsFieldPersonSignature(string cellText) =>
            StampAdditionalParameters.MarkersActionTypeSignature.MarkerContain(cellText);
    }
}
