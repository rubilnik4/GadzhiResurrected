using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.StampContainer
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public class StampMainContainer
    {
        /// <summary>
        /// Доступные поля в Штампе
        /// </summary>
        private IDictionary<string, IElementMicrostation> _stampFields;

        public StampMainContainer(IDictionary<string, IElementMicrostation> stampFields)
        {
            _stampFields = stampFields;
        }

        /// <summary>
        /// Формат штампа
        /// </summary>
        public string Format
        {
            get
            {
                IElementMicrostation elementMicrostation = null;
                _stampFields?.TryGetValue(StampMain.Format, out elementMicrostation);

                return elementMicrostation?.AsTextElementMicrostation()?.Text ?? 
                       String.Empty;
            }
        }
    }
}
