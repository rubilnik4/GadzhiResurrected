using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Implementations.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public abstract partial class StampMicrostation
    {
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override void InsertSignatures()
        {
            StampCellElement.ApplicationMicrostation.AttachLibrary(StampSettingsMicrostation.SignatureLibraryName);

            DeleteSignaturesPrevious();
            InsertSignaturesFromLibrary();

            StampCellElement.ApplicationMicrostation.DetachLibrary();
        }

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected abstract IEnumerable<ICellElementMicrostation> InsertSignaturesFromLibrary();

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        public void DeleteSignaturesPrevious()
        {
            var signaturesElements = StampCellElement.ModelMicrostation.
                                     GetModelElementsMicrostation(ElementMicrostationType.CellElement).
                                     Where(element => element.AttributeControlName == StampFieldMain.SignatureAttributeMarker);

            foreach (var signature in signaturesElements)
            {
                signature.Remove();
            }
        }       

        ///// <summary>
        ///// Вставить и получить подписи из штампа согласования
        ///// </summary>
        //private IEnumerable<ICellElementMicrostation> InsertApprovalSignatures()
        //{
        //    var signatureRowSearch = StampFieldApprovals.GetStampRowApprovalSignatures();
        //    var signatureRowFound = signatureRowSearch?.
        //        Select(row =>
        //            new
        //            {
        //                Person = FindElementInStampFields(row.ResponsiblePerson.Name).AsTextElementMicrostation,
        //                Date = FindElementInStampFields(row.Date.Name).AsTextElementMicrostation,
        //            }).
        //        Where(row => row.Person != null && row.Date != null);

        //    var insertedMainRowSignatures = new List<ICellElementMicrostation>();
        //    foreach (var signature in signatureRowFound)
        //    {
        //        var insertedSignature = InsertSignature(signature.Person.AttributePersonId, signature.Person.Text, signature.Person, signature.Date);
        //        if (insertedSignature != null)
        //        {
        //            insertedMainRowSignatures.Add(insertedSignature);
        //        }
        //    }

        //    return insertedMainRowSignatures;
        //}

        /// <summary>
        /// Вставить подпись
        /// </summary>
        protected ICellElementMicrostation InsertSignature(string personId,
                                                           ITextElementMicrostation previousField, ITextElementMicrostation nextField,
                                                           string personName = null)
        {
            if (previousField == null) throw new ArgumentNullException(nameof(previousField));
            if (nextField == null) throw new ArgumentNullException(nameof(nextField));

            RangeMicrostation signatureRange = GetSignatureRange(StampCellElement.Origin, StampCellElement.UnitScale,
                                                                 previousField, nextField);

            return StampCellElement.ApplicationMicrostation.
                   CreateCellElementFromLibrary(personId, signatureRange.OriginPoint,
                                                StampCellElement.ModelMicrostation,
                                                GetAdditionalParametersToSignature(signatureRange, previousField.IsVertical),
                                                personName);
        }

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private RangeMicrostation GetSignatureRange(PointMicrostation stampOrigin, double unitScale,
                                                    ITextElementMicrostation personField, ITextElementMicrostation dateField)
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
            var signatureRangeInModelCoordinates = signatureRangeInCellCoordinates.Scale(unitScale).
                                                                                   Offset(stampOrigin);
            // левая нижняя точка
            return signatureRangeInModelCoordinates;
        }

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private Action<ICellElementMicrostation> GetAdditionalParametersToSignature(RangeMicrostation signatureRange, bool isVertical) =>       
            new Action<ICellElementMicrostation>(cellElement =>
            {
                cellElement.IsVertical = isVertical;
                PointMicrostation differenceBetweenOriginAndLowLeft = cellElement.Origin.Subtract(cellElement.Range.LowLeftPoint).
                                                                                         Multiply(StampSettingsMicrostation.SignatureRatioMoveFromOriginToLow);
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
                                     new PointMicrostation(signatureRange.Width / cellElement.Range.Width * StampSettingsMicrostation.CompressionRatioText,
                                                           signatureRange.Height / cellElement.Range.Height * StampSettingsMicrostation.CompressionRatioText));

                cellElement.SetAttributeById(ElementMicrostationAttributes.Signature, StampFieldMain.SignatureAttributeMarker);
            });
       
    }
}
