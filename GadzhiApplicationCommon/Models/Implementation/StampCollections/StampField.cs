using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public abstract class StampField : IStampField
    {        
        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }

        public StampField(StampFieldType stampFieldType)
        {           
            StampFieldType = stampFieldType;
        }
    }
}
