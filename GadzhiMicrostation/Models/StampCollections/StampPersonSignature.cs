using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignature
    {
        public StampPersonSignature(string actionType,
                              string responsiblePerson,                              
                              string date)
        {
            ActionType = actionType;
            ResponsiblePerson = responsiblePerson;          
            Date = date;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public string ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public string ResponsiblePerson { get; }
       
        /// <summary>
        /// Дата
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// Список всех полей в строке с ответсвенным лицом и подписью
        /// </summary>
        public HashSet<string> StampPersonSignatureFields => new HashSet<string>()
        {
            ActionType,
            ResponsiblePerson,            
            Date,
        };
    }
}
