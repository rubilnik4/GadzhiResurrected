using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.StampCollections;
using System;

namespace GadzhiMicrostation.Microstation.Implementations.StampFeatures
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public class StampMainFields
    {
        /// <summary>
        /// Штамп
        /// </summary>
        private readonly IStamp _stamp;

        public StampMainFields(IStamp stamp)
        {
            _stamp = stamp;
        }

        /// <summary>
        /// Формат штампа
        /// </summary>
        public string Format => _stamp.FindElementInStampFields(StampMain.Format.Name)?.
                                       AsTextElementMicrostation?.Text ??
                                String.Empty;
    }
}
