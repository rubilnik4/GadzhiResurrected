using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public class StampChange
    {
        public StampChange(string numberChange,
                           string numberOfPlots,
                           string typeOfChange,
                           string documentChange,
                           string dataChange)
        {
            NumberChange = new StampBaseField(numberChange, isNeedCompress: false);
            NumberOfPlots = new StampBaseField(numberOfPlots, isNeedCompress: false);
            TypeOfChange = new StampBaseField(typeOfChange);
            DocumentChange = new StampBaseField(documentChange);
            DateChange = new StampBaseField(dataChange);
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public StampBaseField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public StampBaseField NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public StampBaseField TypeOfChange { get; }

        /// <summary>
        /// Номер докумета
        /// </summary>
        public StampBaseField DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public StampBaseField DateChange { get; }


        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<StampBaseField> StampControlNamesChange => new HashSet<StampBaseField>()
        {
            NumberChange,
            NumberOfPlots,
            TypeOfChange,
            DocumentChange,
            DateChange,
        };
    }
}
