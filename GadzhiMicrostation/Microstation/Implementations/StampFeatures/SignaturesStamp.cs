using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampFeatures;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.StampFeatures
{
    /// <summary>
    /// Класс для работы с подписями
    /// </summary>
    public class SignaturesStamp : ISignaturesStamp
    {
        /// <summary>
        /// Штамп
        /// </summary>
        private readonly IStamp _stamp;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        private readonly IApplicationMicrostation _applicationMicrostation;

        public SignaturesStamp(IStamp stamp)
        {
            _stamp = stamp;
            _applicationMicrostation = _stamp.ApplicationMicrostation;
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertSignatures()
        {
            var signatureRowSearch = StampElement.StampPersonSignatures.StampRowPersonSignatures.
                                                Select(field => field);
            var signatureRowFound = signatureRowSearch?.Select(row =>
                                    new
                                    {
                                        Person = _stamp.FindElementInStampFields(row.ResponsiblePerson.Name).AsTextElementMicrostation,
                                        Date = _stamp.FindElementInStampFields(row.Date.Name).AsTextElementMicrostation,
                                    }).
                                Where(row => row.Person != null && row.Date != null);

            foreach (var signature in signatureRowFound)
            {
                RangeMicrostation signatureRange = GetSignatureRange(signature.Person, signature.Date);
                string personId = signature.Person.GetAttributePersonId();

                _applicationMicrostation.CreateSignatureFromLibrary(personId, signatureRange.OriginPointWithRotation);
            }
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteSignatures()
        {

        }

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private RangeMicrostation GetSignatureRange(ITextElementMicrostation personField, ITextElementMicrostation dateField)
        {
            PointMicrostation leftHeightPoint = new PointMicrostation(personField.OriginPointWithRotationAttributeInUnits.X + personField.WidthAttributeInUnits,
                                                                      personField.OriginPointWithRotationAttributeInUnits.Y);

            PointMicrostation lowRightPoint = new PointMicrostation(dateField.OriginPointWithRotationAttributeInUnits.X,
                                                                    dateField.OriginPointWithRotationAttributeInUnits.Y - dateField.HeightAttributeInUnits);

            return new RangeMicrostation(leftHeightPoint, lowRightPoint, false);
        }
    }
}
