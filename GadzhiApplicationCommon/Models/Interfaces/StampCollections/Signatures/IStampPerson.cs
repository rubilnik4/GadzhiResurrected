using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public interface IStampPerson<out TField> : IStampSignature<TField>
        where TField : IStampField
    {
        /// <summary>
        /// Тип действия
        /// </summary>
        TField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        TField ResponsiblePerson { get; }        

        /// <summary>
        /// Дата
        /// </summary>
        TField DateSignature { get; } 
    }
}
