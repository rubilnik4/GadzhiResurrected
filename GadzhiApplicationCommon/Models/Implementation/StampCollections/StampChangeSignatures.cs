using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public abstract class StampChangeSignature<TField>: IStampChangeSignature<TField> 
                                                         where TField : IStampField
    {
        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public string AttributePersonId { get; }

        public StampChangeSignature(string attributePersonId)
        {
            AttributePersonId = attributePersonId;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public abstract TField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        public abstract TField NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public abstract TField TypeOfChange { get; }

        /// <summary>
        /// Номер докумета
        /// </summary>
        public abstract TField DocumentChange { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public abstract TField Signature { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public abstract TField DateChange { get; }  
    }
}
