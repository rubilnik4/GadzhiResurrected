using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Collections
{
    /// <summary>
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignature
    {
        public StampPersonSignature(string actionType,
                              string responsiblePerson,
                              string signature,
                              string date)
        {
            ActionType = actionType;
            ResponsiblePerson = responsiblePerson;
            Signature = signature;
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
        /// Подпись
        /// </summary>
        public string Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<string> StampPersonSignatureFields => new HashSet<string>()
        {
            ActionType,
            ResponsiblePerson,
            Signature,
            Date,
        };
    }
}
