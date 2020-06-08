using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public abstract class StampField : IStampField
    {
        protected StampField(StampFieldType stampFieldType)
        {
            StampFieldType = stampFieldType;
        }

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }


        /// <summary>
        /// Вертикальное расположение поля
        /// </summary>
        public bool IsVertical => StampFieldType == StampFieldType.ApprovalChangeSignature ||
                                  StampFieldType == StampFieldType.ApprovalSignature;
    }
}
