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
    public interface IStampMain : IStamp
    {
        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        IResultAppCollection<IStampPerson> StampPersons { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        IResultAppCollection<IStampChange> StampChanges { get; }
    }
}
