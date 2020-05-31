﻿using System;
using System.IO;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public class SignatureFileApp : SignatureLibraryApp, ISignatureFileApp
    {
        public SignatureFileApp(string personId, PersonInformationApp personInformation, string signatureFilePath)
            : base(personId, personInformation)
        {
            if (signatureFilePath.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(signatureFilePath));

            SignatureFilePath = signatureFilePath;
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }
    }
}