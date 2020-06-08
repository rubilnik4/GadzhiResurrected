using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Запрос на получение подписи для модуля приложения
    /// </summary>
    public class SignatureFileRequest
    {
        public SignatureFileRequest(string personId, bool isVerticalImage)
        {
            if (personId.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(personId));

            PersonId = personId;
            IsVerticalImage = isVerticalImage;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Вертикальное расположение изображения
        /// </summary>
        public bool IsVerticalImage { get; }
    }
}