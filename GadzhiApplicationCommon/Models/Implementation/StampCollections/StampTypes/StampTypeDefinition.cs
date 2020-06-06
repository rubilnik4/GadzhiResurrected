using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampTypes
{
    /// <summary>
    /// Определение типа штампа
    /// </summary>
    public static class StampTypeDefinition
    {
        /// <summary>
        /// Типы основного штампа
        /// </summary>
        public static IList<StampType> StampMainTypes =>
            new List<StampType>()
            {
                StampType.Full,
                StampType.ChangeNotice,
            };

        /// <summary>
        /// Является ли тип штампа основным
        /// </summary>
        public static bool IsStampTypeMain(StampType stampType) => StampMainTypes.Contains(stampType);

    }
}