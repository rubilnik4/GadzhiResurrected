using System;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    public class SignatureLibrary
    {
        public SignatureLibrary(string id, string fullName, byte[] signatureJpeg)
            : this(id, fullName)
        {
            SignatureJpeg = ValidateSignatureJpeg(signatureJpeg)
                            ? signatureJpeg
                            : throw new ArgumentNullException(nameof(signatureJpeg));
        }

        public SignatureLibrary(string id, string fullName)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Fullname = fullName ?? throw new ArgumentNullException(nameof(fullName));
            SignatureJpeg = null;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string Fullname { get; }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] SignatureJpeg { get; }

        /// <summary>
        /// Проверить корректность данных
        /// </summary>
        public static bool ValidateSignatureJpeg(byte[] signatureJpeg) =>
            signatureJpeg?.Length > 0;
    }
}