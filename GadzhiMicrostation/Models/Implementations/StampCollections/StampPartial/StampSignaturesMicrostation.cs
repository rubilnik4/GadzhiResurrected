using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
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
        public override IResultAppCollection<IStampSignature<IStampField>> InsertSignatures() =>
            StampCellElement.ApplicationMicrostation.
            Void(application => application.AttachLibrary(StampCellElement.ApplicationMicrostation.MicrostationResources.SignatureMicrostationFileName)).
            Void(_ => DeleteSignaturesPrevious()).
            Map(_ => InsertSignaturesFromLibrary()).
            Void(_ => StampCellElement.ApplicationMicrostation.DetachLibrary());

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected abstract IResultAppCollection<IStampSignature<IStampField>> InsertSignaturesFromLibrary();

        /// <summary>
        /// Удалить предыдущие подписи
        /// </summary>
        public Unit DeleteSignaturesPrevious() =>
            StampCellElement.ModelMicrostation.
            GetModelElementsMicrostation(ElementMicrostationType.CellElement).
            Where(element => element.AttributeControlName == StampFieldMain.SignatureAttributeMarker).
            Select(element =>
            {
                element.Remove();
                return Unit.Value;
            }).
            Map(_ => Unit.Value);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        protected IResultAppValue<ICellElementMicrostation> InsertSignature(string personId, ITextElementMicrostation previousField,
                                                                            ITextElementMicrostation nextField, string personName = null) =>
            GetSignatureRange(StampCellElement.Origin, StampCellElement.UnitScale, previousField, nextField).
            ResultValueOkBind(signatureRange => StampCellElement.ApplicationMicrostation.
                                                CreateCellElementFromLibrary(personId, signatureRange.OriginPoint,
                                                                             StampCellElement.ModelMicrostation,
                                                                             CreateSignatureCell(signatureRange, previousField.IsVertical),
                                                                             personName));

        /// <summary>
        /// Получить строки с ответственным лицом/отделом без подписи
        /// </summary>
        protected static IEnumerable<TSignatureField> GetStampSignatureRows<TSignatureField>
                (StampFieldType stampFieldType, Func<IEnumerable<string>, IResultAppValue<TSignatureField>> getSignatureField)
                where TSignatureField : IStampSignatureMicrostation =>
            StampFieldSignatures.GetFieldsBySignatureType(stampFieldType).
            Select(getSignatureField).
            Where(resultSignature => resultSignature.OkStatus && !resultSignature.Value.IsPersonFieldValid()).
            Select(resultApproval => resultApproval.Value);

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>      
        protected Func<string, IResultAppValue<IStampFieldMicrostation>> InsertSignatureFunc(IElementMicrostation previousElement,
                                                                                             IElementMicrostation nextElement,
                                                                                             StampFieldType stampFieldType) =>
            personId =>
                InsertSignature(personId, previousElement.AsTextElementMicrostation, nextElement.AsTextElementMicrostation,
                                previousElement.AsTextElementMicrostation.Text)?.
                ResultValueOk(signature => new StampFieldMicrostation(signature, stampFieldType));

        //Определяется как правая верхняя точка поля Фамилии и как левая нижняя точка Даты
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        private static IResultAppValue<RangeMicrostation> GetSignatureRange(PointMicrostation stampOrigin, double unitScale,
                                                    ITextElementMicrostation previousField, ITextElementMicrostation nextField) =>
            Map(GetSignatureLowLeft(previousField), GetSignatureHighRight(nextField),
                (previous, next) => previous.OkStatus && next.OkStatus
                    ? new ResultAppValue<RangeMicrostation>(new RangeMicrostation(previous.Value, next.Value))
                    : previous.Errors.Concat(next.Errors).ToResultApplicationValue<RangeMicrostation>()).
            ResultValueOk(rangeCellCoordinates => rangeCellCoordinates.Scale(unitScale).
                                                                       Offset(stampOrigin));

        /// <summary>
        /// Получить нижнюю левую точку поля
        /// </summary>       
        private static IResultAppValue<PointMicrostation> GetSignatureLowLeft(ITextElementMicrostation fieldText) =>
          new ResultAppValue<ITextElementMicrostation>(fieldText, new ErrorApplication(ErrorApplicationType.ArgumentNullReference,
                                                                                       "Не задан диапазон поля подписи LowLeft")).
          ResultValueOk(field => !field.IsVertical ?
                                 new PointMicrostation(field.RangeAttributeInUnits.HighRightPoint.X, field.RangeAttributeInUnits.LowLeftPoint.Y) :
                                 new PointMicrostation(field.RangeAttributeInUnits.LowLeftPoint.X, field.RangeAttributeInUnits.HighRightPoint.Y));

        /// <summary>
        /// Получить верхнюю правую точку поля
        /// </summary>       
        private static IResultAppValue<PointMicrostation> GetSignatureHighRight(ITextElementMicrostation fieldText) =>
          new ResultAppValue<ITextElementMicrostation>(fieldText, new ErrorApplication(ErrorApplicationType.ArgumentNullReference,
                                                                                       "Не задан диапазон поля подписи HighRight")).
          ResultValueOk(field => !field.IsVertical ?
                                 new PointMicrostation(field.RangeAttributeInUnits.LowLeftPoint.X, field.RangeAttributeInUnits.HighRightPoint.Y) :
                                 new PointMicrostation(field.RangeAttributeInUnits.HighRightPoint.X, field.RangeAttributeInUnits.LowLeftPoint.Y));

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private static Func<ICellElementMicrostation, ICellElementMicrostation> CreateSignatureCell(RangeMicrostation signatureRange, bool isVertical) =>
            cellElement => cellElement.Copy(isVertical).
                           Map(cell => SignatureMove(cell, signatureRange, isVertical)).
                           Map(cell => SignatureRotate(cell, signatureRange, isVertical)).
                           Map(cell => SignatureScale(cell, signatureRange)).
                           Void(cell => cell.SetAttributeById(ElementMicrostationAttributes.Signature, StampFieldMain.SignatureAttributeMarker));

        /// <summary>
        /// Переместить ячейку подписи
        /// </summary>
        private static ICellElementMicrostation SignatureMove(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange, bool isVertical) =>
            signatureCell.Origin.
            Subtract(signatureCell.Origin).
            WhereOk(_ => isVertical,
                okFunc: originMinusLowLeft => originMinusLowLeft.SubtractY(signatureRange.Width)).
            Map(originMinusLowLeft => (ICellElementMicrostation)signatureCell.Move(originMinusLowLeft));

        /// <summary>
        /// Повернуть ячейку подписи
        /// </summary>       
        private static ICellElementMicrostation SignatureRotate(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange, bool isVertical) =>
            GetRotatePoint(signatureRange, isVertical).
            WhereContinue(_ => isVertical,
                okFunc: rotatePoint => (ICellElementMicrostation)signatureCell.Rotate(rotatePoint, 90),
                badFunc: _ => signatureCell);

        /// <summary>
        /// Масштабировать ячейку подписи
        /// </summary>       
        private static ICellElementMicrostation SignatureScale(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange) =>
            new PointMicrostation(signatureRange.Width / signatureCell.Range.Width * StampSettingsMicrostation.CompressionRatioText,
                                  signatureRange.Height / signatureCell.Range.Height * StampSettingsMicrostation.CompressionRatioText).
            Map(scaleFactor => GetScalePoint(signatureRange).
                               Map(scalePoint => (ICellElementMicrostation)signatureCell.ScaleAll(scalePoint, scaleFactor)));

        /// <summary>
        /// Получить точку для поворота
        /// </summary>
        private static PointMicrostation GetRotatePoint(RangeMicrostation signatureRange, bool isVertical) =>
            signatureRange.
            WhereContinue(_ => isVertical,
                okFunc: range => range.LowLeftPoint,
                badFunc: range => new PointMicrostation(range.HighRightPoint.X, range.LowLeftPoint.Y));

        /// <summary>
        /// Получить точку для масштабирования
        /// </summary>
        private static PointMicrostation GetScalePoint(RangeMicrostation signatureRange) =>
            new PointMicrostation(signatureRange.LowLeftPoint.X + signatureRange.Width / 2,
                                  signatureRange.LowLeftPoint.Y + signatureRange.Height / 2);
    }
}
