using System;
using System.Collections.Generic;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public class SignatureFileData : SignatureLibrary, ISignatureFileData
    {
        public SignatureFileData(string id, PersonInformation personInformation,
                                 IList<byte> signatureFileData, bool isVerticalImage)
            : base(id, personInformation)
        {
            SignatureSource =  signatureFileData;
            IsVerticalImage = isVerticalImage;
        }
        /// <summary>
        /// Изображение подписи
        /// </summary>
        public IList<byte> SignatureSource { get; }

        /// <summary>
        /// Вертикальное расположение изображения
        /// </summary>
        public bool IsVerticalImage { get; }
    }
}
