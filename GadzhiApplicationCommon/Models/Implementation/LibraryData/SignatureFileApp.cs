﻿using System;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и подпись
    /// </summary>
    public class SignatureFileApp : SignatureLibraryApp, ISignatureFileApp
    {
        public SignatureFileApp(string personId, PersonInformationApp personInformation, string signatureFilePath, bool isVerticalImage)
            : base(personId, personInformation)
        {
            SignatureFilePath = signatureFilePath ?? throw new ArgumentNullException(nameof(signatureFilePath));
            IsVerticalImage = isVerticalImage;
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public string SignatureFilePath { get; }

        /// <summary>
        /// Вертикальное расположение изображения
        /// </summary>
        public bool IsVerticalImage { get; }
    }
}
