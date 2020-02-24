using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public partial class Stamp : ISignaturesStamp
    {
        private IEnumerable<ICellElementMicrostation> _insertedSignatures;

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertSignatures()
        {
            var mainRowSignatures = InsertMainRowSignatures();
            var approvalSignatures = InsertApprovalSignatures();

            _insertedSignatures = mainRowSignatures?.Union(approvalSignatures);
        }

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        public void DeleteSignaturesPrevious()
        {
            var signaturesElements = OwnerContainerMicrostation.ModelMicrostation.
                                     GetModelElementsMicrostation(ElementMicrostationType.CellElement).
                                     Where(element => element.AttributeControlName == StampMain.SignatureAttributeMarker);

            foreach (var signature in signaturesElements)
            {
                signature.Remove();
            }
        }

        /// <summary>
        /// Удалить текущие подписи
        /// </summary>
        public void DeleteSignaturesInserted()
        {
            if (_insertedSignatures != null)
            {
                foreach (var signature in _insertedSignatures)
                {
                    signature.Remove();
                }
            }
        }

        /// <summary>
        /// Вставить и получить подписи из основного штампа
        /// </summary>
        private IEnumerable<ICellElementMicrostation> InsertMainRowSignatures()
        {
            var signatureRowSearch = StampPersonSignatures.GetStampRowPersonSignatures();
            var signatureRowFound = signatureRowSearch?.
                Select(row =>
                    new
                    {
                        Person = FindElementInStampFields(row.ResponsiblePerson.Name).AsTextElementMicrostation,
                        Date = FindElementInStampFields(row.Date.Name).AsTextElementMicrostation,
                    }).
                Where(row => row.Person != null && row.Date != null);

            var insertedMainRowSignatures = new List<ICellElementMicrostation>();
            foreach (var signature in signatureRowFound)
            {
                var insertedSignature = InsertSignature(signature.Person, signature.Date);
                insertedMainRowSignatures.Add(insertedSignature);
            }

            return insertedMainRowSignatures;
        }

        /// <summary>
        /// Вставить и получить подписи из штампа согласования
        /// </summary>
        private IEnumerable<ICellElementMicrostation> InsertApprovalSignatures()
        {
            var signatureRowSearch = StampApprovals.GetStampRowApprovalSignatures();
            var signatureRowFound = signatureRowSearch?.
                Select(row =>
                    new
                    {
                        Person = FindElementInStampFields(row.ResponsiblePerson.Name).AsTextElementMicrostation,
                        Date = FindElementInStampFields(row.Date.Name).AsTextElementMicrostation,
                    }).
                Where(row => row.Person != null && row.Date != null);

            var insertedMainRowSignatures = new List<ICellElementMicrostation>();
            foreach (var signature in signatureRowFound)
            {
                var insertedSignature = InsertSignature(signature.Person, signature.Date);
                insertedMainRowSignatures.Add(insertedSignature);
            }

            return insertedMainRowSignatures;
        }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        private ICellElementMicrostation InsertSignature(ITextElementMicrostation person, ITextElementMicrostation date)
        {
            RangeMicrostation signatureRange = GetSignatureRange(Origin, person, date);

            return ApplicationMicrostation.CreateSignatureFromLibrary(person.AttributePersonId,
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
            PointMicrostation lowLeftPoint = new PointMicrostation(personField.OriginPointWithRotationAttributeInUnits.X + personField.WidthAttributeInUnits,
                                                                   personField.OriginPointWithRotationAttributeInUnits.Y);

            PointMicrostation highRightPoint = new PointMicrostation(dateField.OriginPointWithRotationAttributeInUnits.X,
                                                                     dateField.OriginPointWithRotationAttributeInUnits.Y + dateField.HeightAttributeInUnits);

            var signatureRangeInCellCoordinates = new RangeMicrostation(lowLeftPoint, highRightPoint, personField.IsVertical);
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
                cellElement.IsVertical = signatureRange.IsVertical;
                PointMicrostation differenceBetweenOriginAndLowLeft = cellElement.Origin.Subtract(cellElement.LowLeftPoint).
                                                                                         Multiply(StampAdditionalParameters.SignatureRatioMoveFromOriginToLow);
                cellElement.Move(differenceBetweenOriginAndLowLeft);

                cellElement.ScaleAll(cellElement.LowLeftPoint,
                                     new PointMicrostation(signatureRange.Width / cellElement.Width * StampAdditionalParameters.CompressionRatioText,
                                                           signatureRange.Height / cellElement.Height * StampAdditionalParameters.CompressionRatioText));

                cellElement.SetAttributeById(ElementMicrostationAttributes.Signature, StampMain.SignatureAttributeMarker);
            });
        }
    }
}
