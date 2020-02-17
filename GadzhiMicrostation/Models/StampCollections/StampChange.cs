using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
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
            NumberChange = numberChange;
            NumberOfPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;        
            DataChange = dataChange;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public string NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public string NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public string TypeOfChange { get; }

        /// <summary>
        /// Номер докумета
        /// </summary>
        public string DocumentChange { get; }      

        /// <summary>
        /// Дата изменения
        /// </summary>
        public string DataChange { get; }


        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<string> StampControlNamesChange => new HashSet<string>()
        {
            NumberChange,
            NumberOfPlots,
            TypeOfChange,
            DocumentChange,
            DataChange,
        };
    }
}
