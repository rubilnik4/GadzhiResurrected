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
            ApplicationMicrostation.AttachLibrary(StampAdditionalParameters.SignatureLibraryPath);

            var mainRowSignatures = InsertMainRowSignatures();

            IEnumerable<ICellElementMicrostation> changesSignatures = new List<ICellElementMicrostation>();
            if (!String.IsNullOrEmpty(mainRowSignatures?.FirstOrDefault()?.Name))
            {
                var firstMainRowSignature = mainRowSignatures?.FirstOrDefault();
                changesSignatures = InsertChangesSignatures(firstMainRowSignature?.Name, firstMainRowSignature?.Description);
            }
            var approvalSignatures = InsertApprovalSignatures();

            _insertedSignatures = mainRowSignatures?.Union(changesSignatures)?.Union(approvalSignatures);

            ApplicationMicrostation.DetachLibrary();
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
                var insertedSignature = InsertSignature(signature.Person.AttributePersonId, signature.Person.Text, signature.Person, signature.Date);
                if (insertedSignature != null)
                {
                    insertedMainRowSignatures.Add(insertedSignature);
                }
            }

            return insertedMainRowSignatures;
        }

        /// <summary>
        /// Вставить и получить подписи из штампа замены
        /// </summary>
        private IEnumerable<ICellElementMicrostation> InsertChangesSignatures(string personId, string personName)
        {
            var signatureRowSearch = StampChanges.GetStampRowChangesSignatures();
            var signatureRowFound = signatureRowSearch?.
                Select(row =>
                    new
                    {
                        DocumentChange = FindElementInStampFields(row.DocumentChange.Name).AsTextElementMicrostation,
                        Date = FindElementInStampFields(row.DateChange.Name).AsTextElementMicrostation,
                    }).
                Where(row => row.DocumentChange != null && row.Date != null &&
                      !String.IsNullOrEmpty(row.DocumentChange.Text.Trim()));

            var insertedMainRowSignatures = new List<ICellElementMicrostation>();
            foreach (var signature in signatureRowFound)
            {
                var insertedSignature = InsertSignature(personId, personName, signature.DocumentChange, signature.Date);
                if (insertedSignature != null)
                {
                    insertedMainRowSignatures.Add(insertedSignature);
                }
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
                var insertedSignature = InsertSignature(signature.Person.AttributePersonId, signature.Person.Text, signature.Person, signature.Date);
                if (insertedSignature != null)
                {
                    insertedMainRowSignatures.Add(insertedSignature);
                }
            }

            return insertedMainRowSignatures;
        }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        private ICellElementMicrostation InsertSignature(string personId, string personName, ITextElementMicrostation previousField, ITextElementMicrostation nextField)
        {
            RangeMicrostation signatureRange = GetSignatureRange(Origin, previousField, nextField);

            return ApplicationMicrostation.CreateCellElementFromLibrary(personId,
                                                                        signatureRange.OriginPoint,
                                                                        OwnerContainerMicrostation.ModelMicrostation,
                                                                        GetAdditionalParametersToSignature(signatureRange, previousField.IsVertical),
                                                                        personName);
        }

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private RangeMicrostation GetSignatureRange(PointMicrostation stampOrigin,
                                                    ITextElementMicrostation personField,
                                                    ITextElementMicrostation dateField)
        {
            PointMicrostation lowLeftPoint = !personField.IsVertical ?
                                              new PointMicrostation(personField.RangeAttributeInUnits.HighRightPoint.X,
                                                                    personField.RangeAttributeInUnits.LowLeftPoint.Y) :
                                              new PointMicrostation(personField.RangeAttributeInUnits.LowLeftPoint.X,
                                                                    personField.RangeAttributeInUnits.HighRightPoint.Y);

            PointMicrostation highRightPoint = !personField.IsVertical ?
                                                new PointMicrostation(dateField.RangeAttributeInUnits.LowLeftPoint.X,
                                                                      dateField.RangeAttributeInUnits.HighRightPoint.Y) :
                                                new PointMicrostation(dateField.RangeAttributeInUnits.HighRightPoint.X,
                                                                      dateField.RangeAttributeInUnits.LowLeftPoint.Y);

            var signatureRangeInCellCoordinates = new RangeMicrostation(lowLeftPoint, highRightPoint);
            var signatureRangeInModelCoordinates = signatureRangeInCellCoordinates.Scale(UnitScale).
                                                                                   Offset(stampOrigin);
            // левая нижняя точка
            return signatureRangeInModelCoordinates;
        }

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private Action<ICellElementMicrostation> GetAdditionalParametersToSignature(RangeMicrostation signatureRange, bool isVertical)
        {
            return new Action<ICellElementMicrostation>(cellElement =>
            {
                cellElement.IsVertical = isVertical;
                PointMicrostation differenceBetweenOriginAndLowLeft = cellElement.Origin.Subtract(cellElement.Range.LowLeftPoint).
                                                                                         Multiply(StampAdditionalParameters.SignatureRatioMoveFromOriginToLow);
                if (isVertical)
                {
                    differenceBetweenOriginAndLowLeft.X += signatureRange.Width;
                }
                cellElement.Move(differenceBetweenOriginAndLowLeft);

                var originPoint = !isVertical ? cellElement.Range.LowLeftPoint : new PointMicrostation(signatureRange.HighRightPoint.X, signatureRange.LowLeftPoint.Y);
                if (isVertical)
                {
                    cellElement.Rotate(originPoint, 90);
                }

                cellElement.ScaleAll(originPoint,
                                     new PointMicrostation(signatureRange.Width / cellElement.Range.Width * StampAdditionalParameters.CompressionRatioText,
                                                           signatureRange.Height / cellElement.Range.Height * StampAdditionalParameters.CompressionRatioText));

                cellElement.SetAttributeById(ElementMicrostationAttributes.Signature, StampMain.SignatureAttributeMarker);
            });
        }
    }
}
