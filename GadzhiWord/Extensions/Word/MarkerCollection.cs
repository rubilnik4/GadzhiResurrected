using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Extensions.Word
{
    /// <summary>
    /// обработка коллекций маркеров
    /// </summary>
    public static class MarkerCollection
    {
        /// <summary>
        /// Содержит ли коллекиця маркеров указанное поле
        /// </summary>        
        public static bool MarkerContain(this IEnumerable<string> markerCollection, string cellText) =>
            markerCollection?.Any(marker => cellText?.StartsWith(marker, StringComparison.CurrentCulture) == true) == true;
    }
}
