using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiApplicationCommon.Extensions.Functional;

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
        public override IEnumerable<IErrorApplication> InsertSignatures()
        {
            StampCellElement.ApplicationMicrostation.AttachLibrary(StampCellElement.ApplicationMicrostation.
                                                                   MicrostationResources.SignatureMicrostationFileName);
            DeleteSignaturesPrevious();
            //Использование toList обязательно. Для синхронизации выполнения с прикрепленной библиотекой
            IEnumerable<IErrorApplication> signatureErrors = InsertSignaturesFromLibrary().ToList();
            StampCellElement.ApplicationMicrostation.DetachLibrary();

            return signatureErrors;
        }

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected abstract IEnumerable<IErrorApplication> InsertSignaturesFromLibrary();

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

        /// <summary>
        /// Вставить подпись
        /// </summary>
        protected ICellElementMicrostation InsertSignature(string personId,
                                                           ITextElementMicrostation previousField, ITextElementMicrostation nextField,
                                                           string personName = null) =>        
            GetSignatureRange(StampCellElement.Origin, StampCellElement.UnitScale, previousField, nextField).
            Map(signatureRange => StampCellElement.ApplicationMicrostation.
                                  CreateCellElementFromLibrary(personId, signatureRange.OriginPoint,
                                                               StampCellElement.ModelMicrostation,
                                                               CreateSignatureCell(signatureRange, previousField.IsVertical),
                                                               personName));      

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private RangeMicrostation GetSignatureRange(PointMicrostation stampOrigin, double unitScale,
                                                    ITextElementMicrostation previousField, ITextElementMicrostation nextField)
        {
            if (previousField == null) throw new ArgumentNullException(nameof(previousField));
            if (nextField == null) throw new ArgumentNullException(nameof(nextField));

            PointMicrostation lowLeftPoint = !previousField.IsVertical ?
                                              new PointMicrostation(previousField.RangeAttributeInUnits.HighRightPoint.X,
                                                                    previousField.RangeAttributeInUnits.LowLeftPoint.Y) :
                                              new PointMicrostation(previousField.RangeAttributeInUnits.LowLeftPoint.X,
                                                                    previousField.RangeAttributeInUnits.HighRightPoint.Y);

            PointMicrostation highRightPoint = !previousField.IsVertical ?
                                                new PointMicrostation(nextField.RangeAttributeInUnits.LowLeftPoint.X,
                                                                      nextField.RangeAttributeInUnits.HighRightPoint.Y) :
                                                new PointMicrostation(nextField.RangeAttributeInUnits.HighRightPoint.X,
                                                                      nextField.RangeAttributeInUnits.LowLeftPoint.Y);

            var signatureRangeInCellCoordinates = new RangeMicrostation(lowLeftPoint, highRightPoint);
            var signatureRangeInModelCoordinates = signatureRangeInCellCoordinates.Scale(unitScale).
                                                                                   Offset(stampOrigin);
            // левая нижняя точка
            return signatureRangeInModelCoordinates;
        }

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private Func<ICellElementMicrostation, ICellElementMicrostation> CreateSignatureCell(RangeMicrostation signatureRange, bool isVertical) =>
            new Func<ICellElementMicrostation, ICellElementMicrostation>(cellElement =>
            {
                var signatureCell = cellElement.Copy(isVertical);
                PointMicrostation differenceBetweenOriginAndLowLeft = signatureCell.Origin.Subtract(signatureCell.Range.LowLeftPoint).
                                                                                           Multiply(StampSettingsMicrostation.SignatureRatioMoveFromOriginToLow);
                if (isVertical)
                {
                    differenceBetweenOriginAndLowLeft.X += signatureRange.Width;
                }
                signatureCell.Move(differenceBetweenOriginAndLowLeft);

                var originPoint = !isVertical ? signatureCell.Range.LowLeftPoint : new PointMicrostation(signatureRange.HighRightPoint.X, signatureRange.LowLeftPoint.Y);
                if (isVertical)
                {
                    signatureCell.Rotate(originPoint, 90);
                }

                signatureCell.ScaleAll(originPoint,
                                       new PointMicrostation(signatureRange.Width / signatureCell.Range.Width * StampSettingsMicrostation.CompressionRatioText,
                                                           signatureRange.Height / signatureCell.Range.Height * StampSettingsMicrostation.CompressionRatioText));

                signatureCell.SetAttributeById(ElementMicrostationAttributes.Signature, StampFieldMain.SignatureAttributeMarker);
                return signatureCell;
            });

    }
}
