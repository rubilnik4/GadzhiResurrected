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
        private static IDictionary<string, StampFieldBase> StampBaseFields
        {
            get
            {
                var stampControlNames = StampFieldMain.GetStampMainFields();
                stampControlNames.UnionWith(StampFieldPersonSignatures.GetFieldsPersonSignatures());
                stampControlNames.UnionWith(StampFieldChanges.GetFieldsChangeSignatures());
                stampControlNames.UnionWith(StampFieldApprovals.GetFieldsApprovalSignatures());

                return stampControlNames.ToDictionary(p => p.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static HashSet<string> StampControlNames => new HashSet<string>(StampBaseFields.Keys);

        /// <summary>
        /// Содержится ли поле в списке Штампа
        /// </summary>       
        public static bool ContainControlName(string controlName) => !String.IsNullOrEmpty(controlName) && 
                                                                      StampControlNames.Contains(controlName);
          

        /// <summary>
        /// Получить список параметров по имени элемента
        /// </summary>
        public static StampFieldBase GetBaseParametersByControlName(string controlName) => StampBaseFields[controlName];
    }
}
