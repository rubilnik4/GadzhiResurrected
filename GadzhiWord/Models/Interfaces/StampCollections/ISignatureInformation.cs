using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Информация о подписи для модуля Word
    /// </summary>
    public interface ISignatureInformation
    {
        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        string PersonName { get; }

        /// <summary>
        /// Путь к файлу подписи
        /// </summary>
        string SignaturePath { get; }
    }
}
