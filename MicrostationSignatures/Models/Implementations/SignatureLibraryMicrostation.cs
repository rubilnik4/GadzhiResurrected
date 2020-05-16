using System;

namespace MicrostationSignatures.Models.Implementations
{
    public readonly struct SignatureLibraryMicrostation : IEquatable<SignatureLibraryMicrostation>
    {
        public SignatureLibraryMicrostation(string fullname, byte[] signatureMicrostation)
        {
            Fullname = fullname ?? throw new ArgumentNullException(nameof(fullname));
            SignatureMicrostation = ValidateSignatureMicrostation(signatureMicrostation)
                            ? signatureMicrostation
                            : throw new ArgumentNullException(nameof(signatureMicrostation));
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Fullname { get; }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] SignatureMicrostation { get; }

        /// <summary>
        /// Проверить корректность данных
        /// </summary>
        public static bool ValidateSignatureMicrostation(byte[] signatureMicrostation) =>
            signatureMicrostation?.Length > 0;

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((SignatureLibraryMicrostation)obj);

        public bool Equals(SignatureLibraryMicrostation other) => other.Fullname == Fullname;

        public static bool operator ==(SignatureLibraryMicrostation left, SignatureLibraryMicrostation right) => left.Equals(right);

        public static bool operator !=(SignatureLibraryMicrostation left, SignatureLibraryMicrostation right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + Fullname.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}