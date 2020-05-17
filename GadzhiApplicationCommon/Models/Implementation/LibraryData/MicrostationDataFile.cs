using System;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    public class MicrostationDataFile
    {
        public MicrostationDataFile(string nameDatabase, byte[] signatureMicrostation)
        {
            NameDatabase = nameDatabase ?? throw new ArgumentNullException(nameof(nameDatabase));
            MicrostationDataBase = ValidateMicrostationDataFile(signatureMicrostation)
                            ? signatureMicrostation
                            : throw new ArgumentNullException(nameof(signatureMicrostation));
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string NameDatabase { get; }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] MicrostationDataBase { get; }

        /// <summary>
        /// Проверить корректность данных
        /// </summary>
        public static bool ValidateMicrostationDataFile(byte[] signatureMicrostation) =>
            signatureMicrostation?.Length > 0;
    }
}