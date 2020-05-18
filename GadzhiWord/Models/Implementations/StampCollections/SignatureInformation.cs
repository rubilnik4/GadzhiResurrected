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
    public class SignatureInformation: IEquatable<SignatureInformation>, ISignatureInformation
    {
        public SignatureInformation(string personId, string personName)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            PersonName = personName ?? throw new ArgumentNullException(nameof(personName));
        }

        /// <summary>
        /// Идентификатор личности
        /// </summary>    
        public string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public string PersonName { get; }

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((SignatureInformation)obj);

        public bool Equals(SignatureInformation other) => other?.PersonId == PersonId;

        public static bool operator ==(SignatureInformation left, SignatureInformation right) => left?.Equals(right) == true;

        public static bool operator !=(SignatureInformation left, SignatureInformation right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + PersonId.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}
