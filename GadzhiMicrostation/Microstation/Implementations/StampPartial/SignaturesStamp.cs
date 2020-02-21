using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Linq;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public partial class Stamp : ISignaturesStamp
    {
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertSignatures()
        {
            var signatureRowSearch = StampPersonSignatures.GetStampRowPersonSignatures().
                                                Select(field => field);
            var signatureRowFound =signatureRowSearch?.
                Select(row =>
                    new
                    {
                        Person = FindElementInStampFields(row.ResponsiblePerson.Name).AsTextElementMicrostation,
                        Date = FindElementInStampFields(row.Date.Name).AsTextElementMicrostation,
                    }).
                Where(row => row.Person != null && row.Date != null);

            foreach (var signature in signatureRowFound)
            {
                InsertSignature(signature.Person, signature.Date);
            }
        }

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        public void DeleteSignaturesPrevious()
        {
            var signaturesElements = GetSubElements().Where(subElement => subElement.ElementType == ElementMicrostationType.CellElement);

        }

        /// <summary>
        /// Удалить текущие подписи
        /// </summary>
        public void DeleteSignaturesInserted()
        {

        }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        private void InsertSignature(ITextElementMicrostation person, ITextElementMicrostation date)
        {
            RangeMicrostation signatureRange = GetSignatureRange(Origin, person, date);

            // ICellElementMicrostation cellElementMicrostation =
            ApplicationMicrostation.CreateSignatureFromLibrary(person.AttributePersonId,
                                                               signatureRange.OriginPointWithRotation,
                                                               OwnerContainerMicrostation.ModelMicrostation,
                                                               GetAdditionalParametersToSignature(signatureRange));
        }

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private RangeMicrostation GetSignatureRange(PointMicrostation stampOrigin,
                                                    ITextElementMicrostation personField,
                                                    ITextElementMicrostation dateField)
        {
            PointMicrostation leftHeightPoint = new PointMicrostation(personField.OriginPointWithRotationAttributeInUnits.X + personField.WidthAttributeInUnits,
                                                                      personField.OriginPointWithRotationAttributeInUnits.Y + dateField.HeightAttributeInUnits);

            PointMicrostation lowRightPoint = new PointMicrostation(dateField.OriginPointWithRotationAttributeInUnits.X,
                                                                    dateField.OriginPointWithRotationAttributeInUnits.Y);

            var signatureRangeInCellCoordinates = new RangeMicrostation(leftHeightPoint, lowRightPoint, false);
            var signatureRangeInModelCoordinates = signatureRangeInCellCoordinates.Scale(UnitScale).
                                                                                   Offset(stampOrigin);
            // левая нижняя точка
            return signatureRangeInModelCoordinates;
        }

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private Action<ICellElementMicrostation> GetAdditionalParametersToSignature(RangeMicrostation signatureRange)
        {
            return new Action<ICellElementMicrostation>(cellElement =>
            {
                PointMicrostation differenceBetweenOriginAndLowLeft = cellElement.Origin.Subtract(cellElement.LowLeftPoint).
                                                                                         Multiply(StampAdditionalParameters.SignatureRatioMoveFromOriginToLow);
                cellElement.Move(differenceBetweenOriginAndLowLeft);

                cellElement.ScaleAll(cellElement.LowLeftPoint,
                                     new PointMicrostation(signatureRange.Width / cellElement.Width * StampAdditionalParameters.CompressionRatioText,
                                                           signatureRange.Height / cellElement.Height * StampAdditionalParameters.CompressionRatioText));

                cellElement.SetAttributeById(ElementMicrostationAttributes.Signature, StampAdditionalParameters.SignatureAttributeMarker);
            });
        }
    }
}
