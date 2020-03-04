using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
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
            ActionType = new StampBaseField(actionType, isNeedCompress: false);
            ResponsiblePerson = new StampBaseField(responsiblePerson);
            Date = new StampBaseField(date);
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public StampBaseField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public StampBaseField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampBaseField Date { get; }

        /// <summary>
        /// Список всех полей в строке с ответсвенным лицом и подписью
        /// </summary>
        public HashSet<StampBaseField> StampPersonSignatureFields => new HashSet<StampBaseField>()
        {
            ActionType,
            ResponsiblePerson,
            Date,
        };
    }
}
