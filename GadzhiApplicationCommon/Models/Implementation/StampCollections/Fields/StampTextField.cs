using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Базовая текстовая ячейка штампа
    /// </summary>  
    public abstract class StampTextField : StampField, IStampTextField
    {
        protected StampTextField(StampFieldType stampFieldType)
            : base(stampFieldType)
        { }

        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        public abstract string MaxLengthWord { get; }
    }
}