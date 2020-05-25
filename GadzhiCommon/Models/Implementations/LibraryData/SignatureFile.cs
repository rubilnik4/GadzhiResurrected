using System;
using System.IO;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public class SignatureFile : SignatureLibrary, ISignatureFile
    {
        public SignatureFile(string personId, PersonInformation personInformation, string signatureFolderPath)
            : base(personId, personInformation)
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
