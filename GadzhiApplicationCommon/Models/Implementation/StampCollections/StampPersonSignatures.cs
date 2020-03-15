using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public abstract class StampPersonSignature<TField>: IStampPersonSignature<TField> 
                                                        where TField : IStampField
    {
        public StampPersonSignature()
        {

        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public abstract TField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public abstract TField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public abstract TField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public abstract TField DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public abstract string AttributePersonId { get; }
         
    }
}
