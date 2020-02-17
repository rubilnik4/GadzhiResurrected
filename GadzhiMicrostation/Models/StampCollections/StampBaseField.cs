using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Базовые параметры для поля в штампе
    /// </summary>
    public class StampBaseField
    {
        public StampBaseField(string name, 
                              bool isNeedCompress = true,
                              bool isVertical = false)
        {
            Name = name;
            IsNeedCompress = isNeedCompress;
            IsVertical = isVertical;
        }

        /// <summary>
        /// Имя поля
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        public bool IsNeedCompress { get; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; }
    }
}
