using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public interface IStampChangeSignature<out TField> where TField : IStampField
    {
        /// <summary>
        /// Номер изменения
        /// </summary>
        TField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        TField NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        TField TypeOfChange { get; }

        /// <summary>
        /// Номер докумета
        /// </summary>
        TField DocumentChange { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        TField Signature { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        TField DateChange { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        string AttributePersonId { get; }
    }
}
