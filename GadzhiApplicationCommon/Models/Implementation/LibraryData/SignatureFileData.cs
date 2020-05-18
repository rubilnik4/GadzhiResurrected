using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.StringAdditional;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public class SignatureFileData : SignatureLibrary
    {
        public SignatureFileData(string id, string fullName, byte[] signatureFileData)
            : base(id, fullName)
        {
            SignatureFileDataSource = ValidateSignatureFileDate(signatureFileData)
                ? signatureFileData
                : throw new ArgumentNullException(nameof(signatureFileData));
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] SignatureFileDataSource { get; }

        /// <summary>
        /// Проверить корректность данных
        /// </summary>
        public static bool ValidateSignatureFileDate(byte[] signatureFileData) => signatureFileData?.Length > 0;
    }
}
