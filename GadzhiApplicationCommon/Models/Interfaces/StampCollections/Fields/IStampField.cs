using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>  
    public interface IStampField
    {
        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        StampFieldType StampFieldType { get; }

        /// <summary>
        /// Вертикальное расположение подписи
        /// </summary>
        bool IsVertical { get; }
    }
}
