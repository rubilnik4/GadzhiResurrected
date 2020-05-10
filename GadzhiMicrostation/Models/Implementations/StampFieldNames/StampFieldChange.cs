using System.Collections.Generic;
using GadzhiMicrostation.Models.Interfaces.StampFieldNames;

namespace GadzhiMicrostation.Models.Implementations.StampFieldNames
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public class StampFieldChange : IStampFieldSignature
    {
        public StampFieldChange(string numberChange, string numberOfPlots, string typeOfChange, string documentChange,
                                string dataChange)
        {
            NumberChange = new StampFieldBase(numberChange, isNeedCompress: false);
            NumberOfPlots = new StampFieldBase(numberOfPlots, isNeedCompress: false);
            TypeOfChange = new StampFieldBase(typeOfChange);
            DocumentChange = new StampFieldBase(documentChange);
            DateChange = new StampFieldBase(dataChange);
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public StampFieldBase NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public StampFieldBase NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public StampFieldBase TypeOfChange { get; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public StampFieldBase DocumentChange { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public StampFieldBase DateChange { get; }

        /// <summary>
        /// Список всех полей
        /// </summary>
        public HashSet<StampFieldBase> StampSignatureFields => new HashSet<StampFieldBase>()
        {
            NumberChange,
            NumberOfPlots,
            TypeOfChange,
            DocumentChange,
            DateChange,
        };
    }
}
