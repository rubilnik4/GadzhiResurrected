using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampAdditionalParameters
    {
        /// <summary>
        /// Маркеры основного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersMainStamp => new List<string>()
        {
            "Разраб",
            "Исполн",
            "Составил",
        };

        /// <summary>
        /// Маркеры дополнительного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersAdditionalStamp => new List<string>()
        {
            "Лист",
        };
    }
}
