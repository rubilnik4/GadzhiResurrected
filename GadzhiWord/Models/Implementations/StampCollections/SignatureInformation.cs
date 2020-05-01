using GadzhiWord.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Информация о подписи для модуля Word
    /// </summary>
    public class SignatureInformation : ISignatureInformation
    {
        public SignatureInformation(string personId, string personName, string signaturePath)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            PersonName = personName ?? throw new ArgumentNullException(nameof(personName));
            SignaturePath = signaturePath ?? throw new ArgumentNullException(nameof(signaturePath));
        }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public string PersonName { get; }

        /// <summary>
        /// Путь к файлу подписи
        /// </summary>
        public string SignaturePath { get; }
    }
}
