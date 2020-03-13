using GadzhiApplicationCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>  
    public interface IStampField
    {      
        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        StampFieldType StampFieldType { get; }
    }
}
