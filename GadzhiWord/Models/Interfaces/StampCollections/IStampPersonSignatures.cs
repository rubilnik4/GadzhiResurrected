using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public interface IStampPersonSignature
    {
        /// <summary>
        /// Тип действия
        /// </summary>
        IStampField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        IStampField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        IStampField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        IStampField DateSignature { get; }
    }
}
