using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiWord.Models.Implementations.StampCollections;

namespace GadzhiWord.Extensions.Word
{
    /// <summary>
    /// обработка коллекций маркеров
    /// </summary>
    public static class MarkerExtensions
    {
        /// <summary>
        /// Содержит ли коллекция маркеров указанное поле
        /// </summary>        
        public static bool MarkerContain(this IEnumerable<string> markerCollection, string cellText) =>
            StampMarkersWord.MarkerContain(markerCollection, cellText);
    }
}
