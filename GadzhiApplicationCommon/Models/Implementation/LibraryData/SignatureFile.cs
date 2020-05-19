using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public class SignatureFile : SignatureLibrary, ISignatureFile
    {
        public SignatureFile(string id, string fullName, string signatureFilePath)
            : base(id, fullName)
        {
            SignatureFilePath =!signatureFilePath.IsNullOrWhiteSpace()
                                   ? signatureFilePath 
                                   : throw new ArgumentNullException(nameof(signatureFilePath));
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }

        /// <summary>
        /// Формат хранения файла
        /// </summary>
        public static string SaveFormat => "jpg";
    }
}
