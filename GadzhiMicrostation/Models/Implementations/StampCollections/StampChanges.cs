using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Поля штампа с изменениями
    /// </summary>
    public static class StampChanges
    {  
        /// <summary>
        /// Изменение 1
        /// </summary>
        public static StampChange StampChangesFirst =>
            new StampChange("G_E_S_41",
                            "G_E_S_42",
                            "G_E_S_43",
                            "G_E_S_44",
                            "G_E_S_46");

        /// <summary>
        /// Изменение 2
        /// </summary>
        public static StampChange StampChangesSecond =>
            new StampChange("G_E_S_31",
                            "G_E_S_32",
                            "G_E_S_33",
                            "G_E_S_34",
                            "G_E_S_36");

        /// <summary>
        /// Изменение 3
        /// </summary>
        public static StampChange StampChangesThird =>
            new StampChange("G_E_S_21",
                            "G_E_S_22",
                            "G_E_S_23",
                            "G_E_S_24",
                            "G_E_S_26");

        /// <summary>
        /// Изменение 4
        /// </summary>
        public static StampChange StampChangesFourth =>
            new StampChange("G_E_S_11",
                            "G_E_S_12",
                            "G_E_S_13",
                            "G_E_S_14",
                            "G_E_S_16");

        /// <summary>
        /// Список всех полей с изменениями
        /// </summary>
        public static HashSet<StampBaseField> GetStampControlNamesChanges()
        {
            var stampControlNames = new HashSet<StampBaseField>();

            stampControlNames.UnionWith(StampChangesFirst.StampControlNamesChange);
            stampControlNames.UnionWith(StampChangesSecond.StampControlNamesChange);
            stampControlNames.UnionWith(StampChangesThird.StampControlNamesChange);
            stampControlNames.UnionWith(StampChangesFourth.StampControlNamesChange);

            return stampControlNames;
        }

        /// <summary>
        /// Список строк с изменениями
        /// </summary>
        public static HashSet<StampChange> GetStampRowChangesSignatures()
        {
            return new HashSet<StampChange>()
                {
                    StampChangesFirst,
                    StampChangesSecond,
                    StampChangesThird,
                    StampChangesFourth
                };
        }
    }
}
