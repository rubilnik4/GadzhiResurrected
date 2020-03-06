using GadzhiWord.Models.Implementations.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Штамп
    /// </summary>
    public interface IStampWord
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        IEnumerable<IStamp> Stamps { get; }
        
        /// <summary>
        /// Получить список всех строк с подписями во всех штампах
        /// </summary>     
        IEnumerable<IStampPersonSignature> GetStampPersonSignatures();

        /// <summary>
        /// Корретность загрузки штампов
        /// </summary>
        bool IsValid { get; }
    }
}
