using System;
using System.IO;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public class SignatureFile : SignatureLibrary, ISignatureFile
    {
        public SignatureFile(string personId, PersonInformation personInformation, string signatureFilePath)
            : base(personId, personInformation)
        {
            if (String.IsNullOrWhiteSpace(signatureFilePath)) throw new ArgumentNullException(nameof(signatureFilePath));
            if (Path.GetExtension(signatureFilePath) != SaveFormat) throw new FileNotFoundException(nameof(signatureFilePath));

            SignatureFilePath = signatureFilePath;
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }

        /// <summary>
        /// Формат хранения файла
        /// </summary>
        public static string SaveFormat => ".jpg";

        /// <summary>
        /// Сформировать путь для сохранения подписи
        /// </summary>
        public static string GetFilePathByFolder(string signatureFolderPath, string personId) =>
            FileSystemOperations.CombineFilePath(signatureFolderPath, personId, SaveFormat);
    }
}
