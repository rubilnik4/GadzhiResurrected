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
using GadzhiMicrostation.Microstation.Implementations.Elements;
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
            Map(application => application.AttachLibrary(StampCellElement.ApplicationMicrostation.MicrostationResources.SignatureMicrostationFileName)).
            ResultVoidOk(_ => DeleteSignaturesPrevious()).
            ResultValueOkBind(InsertSignaturesFromLibrary).
            ResultVoidOk(_ => StampCellElement.ApplicationMicrostation.DetachLibrary()).
            ToResultCollection();

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        protected abstract IResultAppCollection<IStampSignature<IStampField>> InsertSignaturesFromLibrary(IList<LibraryElement> libraryElements);

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
        protected IResultAppValue<ICellElementMicrostation> InsertSignature(IList<LibraryElement> libraryElements, string personId,
                                                                            ITextElementMicrostation previousField,
                                                                            ITextElementMicrostation nextField, string personName = null) =>
            StampSignatureRange.GetSignatureRange(StampCellElement.GetSubElementsByType(ElementMicrostationType.LineElement).
                                                                   Select(element => element.AsLineElementMicrostation).
                                                                   Map(lineElements => new ResultAppCollection<ILineElementMicrostation>(lineElements)),
                                                  previousField, nextField, previousField.IsVertical).
            ResultValueOkBind(signatureRange => StampCellElement.ApplicationMicrostation.
                                                CreateCellElementFromLibrary(libraryElements, personId, signatureRange.OriginPoint,
                                                                             StampCellElement.ModelMicrostation,
                                                                             CreateSignatureCell(signatureRange, previousField.IsVertical),
                                                                             personName));

        /// <summary>
        /// Получить строки с ответственным лицом/отделом без подписи
        /// </summary>
        protected static IEnumerable<TSignatureField> GetStampSignatureRows<TSignatureField>(StampFieldType stampFieldType,
                                                                                             Func<IEnumerable<string>, IResultAppValue<TSignatureField>> getSignatureField)
                where TSignatureField : IStampSignatureMicrostation =>
            StampFieldSignatures.GetFieldsBySignatureType(stampFieldType).
            Select(getSignatureField).
            Where(resultSignature => resultSignature.OkStatus && resultSignature.Value.IsPersonFieldValid()).
            Select(resultSignature => resultSignature.Value).
            WhereContinue(signatures => signatures.Any(),
                          okFunc: signatures => signatures,
                          badFunc: signatures => null);

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>      
        protected Func<IList<LibraryElement>, string, string, IResultAppValue<IStampFieldMicrostation>> InsertSignatureFunc
            (IElementMicrostation previousElement, IElementMicrostation nextElement, StampFieldType stampFieldType) =>
            (libraryElements, personId, personName) =>
                InsertSignature(libraryElements, personId, previousElement.AsTextElementMicrostation,
                                nextElement.AsTextElementMicrostation, personName).
                ResultValueOk(signature => new StampFieldMicrostation(signature, stampFieldType));

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private static Func<ICellElementMicrostation, ICellElementMicrostation> CreateSignatureCell(RangeMicrostation signatureRange,
                                                                                                    bool isVertical) =>
            cellElement => cellElement.Clone(isVertical).
                           Map(cell => SignatureMove(cell, signatureRange)).
                           Map(cell => SignatureRotate(cell, isVertical)).
                           Map(cell => SignatureScale(cell, signatureRange)).
                           Void(cell => cell.SetAttributeById(ElementMicrostationAttributes.Signature, StampFieldMain.SignatureAttributeMarker));

        /// <summary>
        /// Переместить ячейку подписи
        /// </summary>
        private static ICellElementMicrostation SignatureMove(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange) =>
            signatureRange.OriginCenter.
                          Subtract(signatureCell.Range.OriginCenter).
            Map(signatureCell.Move);

        /// <summary>
        /// Повернуть ячейку подписи
        /// </summary>       
        private static ICellElementMicrostation SignatureRotate(ICellElementMicrostation signatureCell, bool isVertical) =>
            signatureCell.
            WhereContinue(_ => isVertical,
                okFunc: rotatePoint => signatureCell.Rotate(signatureCell.Range.OriginCenter, 90),
                badFunc: _ => signatureCell);

        /// <summary>
        /// Масштабировать ячейку подписи
        /// </summary>       
        private static ICellElementMicrostation SignatureScale(ICellElementMicrostation signatureCell, RangeMicrostation signatureRange) =>
            new PointMicrostation(signatureRange.Width / signatureCell.Range.Width * StampSettingsMicrostation.CompressionRatioText,
                                  signatureRange.Height / signatureCell.Range.Height * StampSettingsMicrostation.CompressionRatioText).
            Map(scaleFactor => GetScalePoint(signatureRange).
                               Map(scalePoint => signatureCell.ScaleAll(scalePoint, scaleFactor)));

        /// <summary>
        /// Получить точку для масштабирования
        /// </summary>
        private static PointMicrostation GetScalePoint(RangeMicrostation signatureRange) =>
            new PointMicrostation(signatureRange.LowLeftPoint.X + signatureRange.Width / 2,
                                  signatureRange.LowLeftPoint.Y + signatureRange.Height / 2);
    }
}
