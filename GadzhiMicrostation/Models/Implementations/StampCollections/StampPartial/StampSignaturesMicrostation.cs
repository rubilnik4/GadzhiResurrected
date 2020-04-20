using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using static GadzhiApplicationCommon.Functional.MapFunctional;

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
        public override IResultApplication InsertSignatures() =>
            StampCellElement.ApplicationMicrostation.
            Void(application => application.AttachLibrary(StampCellElement.ApplicationMicrostation.
                                                          MicrostationResources.SignatureMicrostationFileName)).
            Void(_ => DeleteSignaturesPrevious()).
            Map(_ => InsertSignaturesFromLibrary().ExecuteLazy()).
            Void(_ => StampCellElement.ApplicationMicrostation.DetachLibrary());

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected abstract IResultApplication InsertSignaturesFromLibrary();

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        public Unit DeleteSignaturesPrevious() =>
            StampCellElement.ModelMicrostation.
            GetModelElementsMicrostation(ElementMicrostationType.CellElement).
            Where(element => element.AttributeControlName == StampFieldMain.SignatureAttributeMarker).
            Select(element => { element.Remove(); return Unit.Value; }).
            Map(_ => Unit.Value);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        protected IResultAppValue<ICellElementMicrostation> InsertSignature(string personId,
                                                                                    ITextElementMicrostation previousField,
                                                                                    ITextElementMicrostation nextField,
                                                                                    string personName = null) =>
            GetSignatureRange(StampCellElement.Origin, StampCellElement.UnitScale, previousField, nextField).
            ResultValueOkBind(signatureRange => StampCellElement.ApplicationMicrostation.
                                                CreateCellElementFromLibrary(personId, signatureRange.OriginPoint,
                                                                             StampCellElement.ModelMicrostation,
                                                                             CreateSignatureCell(signatureRange, previousField.IsVertical),
                                                                             personName));

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private IResultAppValue<RangeMicrostation> GetSignatureRange(PointMicrostation stampOrigin, double unitScale,
                                                    ITextElementMicrostation previousField, ITextElementMicrostation nextField) =>
        
            Map(GetSignatureLowLeft(previousField), GetSignatureHighRight(nextField),
                (previous, next) => previous.OkStatus && next.OkStatus ?
                                    new ResultAppValue<RangeMicrostation>(new RangeMicrostation(previous.Value, next.Value)) :
                                    previous.Errors.Concat(next.Errors).ToResultApplicationValue<RangeMicrostation>()).
            ResultValueOk(rangeCellCoordinates => rangeCellCoordinates.Scale(unitScale).
                                                                      Offset(stampOrigin));

        /// <summary>
        /// Получить нижнюю левую точку поля
        /// </summary>       
        private IResultAppValue<PointMicrostation> GetSignatureLowLeft(ITextElementMicrostation fieldText) =>
          new ResultAppValue<ITextElementMicrostation>(fieldText,
                                     new ErrorApplication(ErrorApplicationType.ArgumentNullReference, "Не задан диапазон поля подписи LowLeft")).
          ResultValueOk(field => !field.IsVertical ?
                                 new PointMicrostation(field.RangeAttributeInUnits.HighRightPoint.X, field.RangeAttributeInUnits.LowLeftPoint.Y) :
                                 new PointMicrostation(field.RangeAttributeInUnits.LowLeftPoint.X, field.RangeAttributeInUnits.HighRightPoint.Y));

        /// <summary>
        /// Получить верхнюю правую точку поля
        /// </summary>       
        private IResultAppValue<PointMicrostation> GetSignatureHighRight(ITextElementMicrostation fieldText) =>
          new ResultAppValue<ITextElementMicrostation>(fieldText,
                                     new ErrorApplication(ErrorApplicationType.ArgumentNullReference, "Не задан диапазон поля подписи HighRight")).
          ResultValueOk(field => !field.IsVertical ?
                                 new PointMicrostation(field.RangeAttributeInUnits.LowLeftPoint.X, field.RangeAttributeInUnits.HighRightPoint.Y) :
                                 new PointMicrostation(field.RangeAttributeInUnits.HighRightPoint.X, field.RangeAttributeInUnits.LowLeftPoint.Y));

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private Func<ICellElementMicrostation, ICellElementMicrostation> CreateSignatureCell(RangeMicrostation signatureRange, bool isVertical) =>
            new Func<ICellElementMicrostation, ICellElementMicrostation>(cellElement =>
                cellElement.Copy(isVertical).
                Map(cell => SignatureMove(cell, signatureRange, isVertical)).
                Map(cell => SignatureRotate(cell, signatureRange, isVertical)).
                Map(cell => SignatureScale(cell, signatureRange, isVertical)).
                Void(cell => cell.SetAttributeById(ElementMicrostationAttributes.Signature, StampFieldMain.SignatureAttributeMarker))
            );

        /// <summary>
        /// Переместить ячейку подписи
        /// </summary>
        private ICellElementMicrostation SignatureMove(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange, bool isVertical) =>
            signatureCell.Origin.
            Subtract(signatureCell.Range.LowLeftPoint).
            Multiply(StampSettingsMicrostation.SignatureRatioMoveFromOriginToLow).
            WhereOk(_ => isVertical,
                okFunc: originMinusLowLeft => originMinusLowLeft.AddX(signatureRange.Width)).
            Map(originMinusLowLeft => (ICellElementMicrostation)signatureCell.Move(originMinusLowLeft));

        /// <summary>
        /// Повернуть ячейку подписи
        /// </summary>       
        private ICellElementMicrostation SignatureRotate(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange, bool isVertical) =>
            GetTransformPoint(signatureRange, isVertical).
            WhereContinue(_ => isVertical,
                okFunc: rotatePoint => (ICellElementMicrostation)signatureCell.Rotate(rotatePoint, 90),
                badFunc: _ => signatureCell);

        /// <summary>
        /// Масштабировать ячейку подписи
        /// </summary>       
        private ICellElementMicrostation SignatureScale(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange, bool isVertical) =>
             new PointMicrostation(signatureRange.Width / signatureCell.Range.Width * StampSettingsMicrostation.CompressionRatioText,
                                   signatureRange.Height / signatureCell.Range.Height * StampSettingsMicrostation.CompressionRatioText).
             Map(scaleFactor => GetTransformPoint(signatureRange, isVertical).
                                Map(scalePoint => (ICellElementMicrostation)signatureCell.ScaleAll(scalePoint, scaleFactor)));

        /// <summary>
        /// Получить точку для модификации
        /// </summary>
        private PointMicrostation GetTransformPoint(RangeMicrostation signatureRange, bool isVertical) =>
            signatureRange.
            WhereContinue(_ => isVertical,
                okFunc: range => range.LowLeftPoint,
                badFunc: range => new PointMicrostation(range.HighRightPoint.X, range.LowLeftPoint.Y));
    }
}
