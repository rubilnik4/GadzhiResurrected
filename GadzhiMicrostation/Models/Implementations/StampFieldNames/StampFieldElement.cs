using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Поля составляющие штамп
    /// </summary>
    public static class StampFieldElement
    {
        /// <summary>
        /// Список всех полей
        /// </summary>
        private static IDictionary<string, StampFieldBase> StampBaseParameters
        {
            get
            {
                var stampControlNames = StampFieldMain.GetStampMainFields();
                stampControlNames.UnionWith(StampFieldPersonSignatures.GetFieldsPersonSignatures());
                stampControlNames.UnionWith(StampFieldChanges.GetStampControlNamesChanges());
                stampControlNames.UnionWith(StampFieldApprovals.GetStampControlNamesApprovals());

                return stampControlNames.ToDictionary(p => p.Name);
            }
        }

        private static HashSet<string> StampControlNames => new HashSet<string>(StampBaseParameters.Keys);

        /// <summary>
        /// Содержится ли поле в списке Штампа
        /// </summary>       
        public static bool ContainControlName(string controlName)
        {
            if (!string.IsNullOrEmpty(controlName))
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
        public static StampFieldBase GetBaseParametersByControlName(string controlName) => StampBaseParameters[controlName];
    }
}
