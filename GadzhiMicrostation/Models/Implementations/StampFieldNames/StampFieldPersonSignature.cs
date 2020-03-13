using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampFieldPersonSignature
    {
        public StampFieldPersonSignature(string actionType,
                              string responsiblePerson,
                              string date)
        {
            ActionType = new StampFieldBase(actionType, isNeedCompress: false);
            ResponsiblePerson = new StampFieldBase(responsiblePerson);
            DateSignature = new StampFieldBase(date);
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public StampFieldBase ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public StampFieldBase ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampFieldBase DateSignature { get; }

        /// <summary>
        /// Список всех полей в строке с ответсвенным лицом и подписью
        /// </summary>
        public HashSet<StampFieldBase> StampPersonSignatureFields => new HashSet<StampFieldBase>()
        {
            ActionType,
            ResponsiblePerson,
            DateSignature,
        };
    }
}
