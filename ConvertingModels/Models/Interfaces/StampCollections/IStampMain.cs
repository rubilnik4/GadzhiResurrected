using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.StampCollections
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
