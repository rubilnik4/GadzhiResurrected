using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Поля составляющие штамп
    /// </summary>
    public static class StampElement
    {
        /// <summary>
        /// Основные поля штампа
        /// </summary>
        public static StampMain StampMain => new StampMain();

        /// <summary>
        /// Поля штампа с ответсвенным лицом и подписью
        /// </summary>
        public static StampPersonSignatures StampPersonSignatures => new StampPersonSignatures();

        /// <summary>
        /// Поля штампа с изменениями
        /// </summary>
        public static StampChanges StampChanges => new StampChanges();

        /// <summary>
        /// Поля штампа с согласованием
        /// </summary>
        public static StampApprovals StampApprovals => new StampApprovals();

        /// <summary>
        /// Список всех полей
        /// </summary>
        private static HashSet<string> _stampControlNames
        {
            get
            {
                var stampControlNames = StampMain.StampMainFields;
                stampControlNames.UnionWith(StampPersonSignatures.StampFieldsPersonSignatures);
                stampControlNames.UnionWith(StampChanges.StampControlNamesChanges);
                stampControlNames.UnionWith(StampApprovals.StampControlNamesApprovals);

                return stampControlNames;
            }
        }


        /// <summary>
        /// Содержится ли поле в списке Штампа
        /// </summary>       
        public static bool ContainControlName(string controlName)
        {           
            if (!String.IsNullOrEmpty(controlName))
            {
                if (_stampControlNames.Contains(controlName))
                {
                    return true;
                }
            }
            return false;
        }

       
    }
}
