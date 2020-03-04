using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Поля составляющие штамп
    /// </summary>
    public static class StampElement
    {  
        /// <summary>
        /// Список всех полей
        /// </summary>
        private static IDictionary<string, StampBaseField> StampBaseParameters
        {
            get
            {
                var stampControlNames = StampMain.GetStampMainFields();
                stampControlNames.UnionWith(StampPersonSignatures.GetStampFieldsPersonSignatures());
                stampControlNames.UnionWith(StampChanges.GetStampControlNamesChanges());
                stampControlNames.UnionWith(StampApprovals.GetStampControlNamesApprovals());

                return stampControlNames.ToDictionary(p => p.Name);
            }
        }

        private static HashSet<String> StampControlNames => new HashSet<string>(StampBaseParameters.Keys);

        /// <summary>
        /// Содержится ли поле в списке Штампа
        /// </summary>       
        public static bool ContainControlName(string controlName)
        {
            if (!String.IsNullOrEmpty(controlName))
            {
                if (StampControlNames.Contains(controlName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить список параметров по имени элемента
        /// </summary>
        public static StampBaseField GetBaseParametersByControlName(string controlName) => StampBaseParameters[controlName];
    }
}
