using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Базовые поля штампа
    /// </summary>
    public static class StampFieldBasic
    {
        /// <summary>
        /// Основной шифр
        /// </summary>
        public static StampFieldBase FullCode => new StampFieldBase("G_E_FULLCODE_1");

        /// <summary>
        /// Текущий лист
        /// </summary>
        public static StampFieldBase CurrentSheet => new StampFieldBase("G_E_LISTNUM_1", isNeedCompress: false);

        /// <summary>
        /// Список всех базовых полей
        /// </summary>
        public static HashSet<StampFieldBase> GetStampBasicFields() =>
            new HashSet<StampFieldBase>()
            {
                FullCode, 
                CurrentSheet,
            };
    }
}