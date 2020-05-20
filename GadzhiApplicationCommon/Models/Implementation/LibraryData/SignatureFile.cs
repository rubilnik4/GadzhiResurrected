using System;
using System.Collections.Generic;
using System.IO;
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
        public SignatureFile(string personId, string personName, string signatureFolderPath)
            : base(personId, personName)
        {
            if (signatureFolderPath == null) throw new ArgumentNullException(nameof(signatureFolderPath));
            SignatureFilePath = GetFilePathByFolder(signatureFolderPath, personId);
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }

        /// <summary>
        /// Формат хранения файла
        /// </summary>
        public static string SaveFormat => "jpg";

        /// <summary>
        /// Сформировать путь для сохранения подписи
        /// </summary>
        public static string GetFilePathByFolder(string signatureFolderPath, string personId) =>
            Path.Combine(signatureFolderPath, personId + "." + SaveFormat);
    }
}
