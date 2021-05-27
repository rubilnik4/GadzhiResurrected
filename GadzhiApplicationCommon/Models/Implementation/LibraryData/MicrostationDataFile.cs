using System;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    public class MicrostationDataFile
    {
        public MicrostationDataFile(string nameDatabase, byte[] signatureMicrostation)
        {
            NameDatabase = nameDatabase;
            MicrostationDataBase = signatureMicrostation;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string NameDatabase { get; }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] MicrostationDataBase { get; }
    }
}