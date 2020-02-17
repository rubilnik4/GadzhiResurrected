using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Collections
{
    /// <summary>
    /// Изменения в штампе
    /// </summary>
    public class StampChanges
    {
        public StampChanges(string numberChange,
                            string numberOfPlots,
                            string typeOfChange,
                            string documentChange,
                            string signature,
                            string dataChange)
        {
            NumberChange = numberChange;
            NumberOfPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;
            Signature = signature;
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
        /// Подпись
        /// </summary>
        public string Signature { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public string DataChange { get; }


        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<string> StampChangesFields => new HashSet<string>()
        {
            NumberChange,
            NumberOfPlots,
            TypeOfChange,
            DocumentChange,
            DataChange,
        };
    }
}
