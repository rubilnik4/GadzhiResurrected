using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public interface IStampMain<TField> : IStamp
        where TField : IStampField
    {
        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        IResultAppCollection<IStampPerson<TField>> StampPersons { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        IResultAppCollection<IStampChange<TField>> StampChanges { get; }
    }
}
