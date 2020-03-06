using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public interface IStampMain : IStamp
    {
        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        IEnumerable<IStampPersonSignature> StampPersonSignatures { get; }
    }
}
