using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Базовая текстовая ячейка штампа
    /// </summary>  
    public abstract class StampTextField : StampField, IStampTextField
    {
        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public abstract string Text { get; }
    }
}