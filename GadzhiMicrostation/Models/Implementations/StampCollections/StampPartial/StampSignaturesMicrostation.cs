using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Functional;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating;
using GadzhiMicrostation.Models.Implementations.StampCollections.Signatures;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями Microstation
    /// </summary>
    public abstract partial class StampMicrostation
    {
        /// <summary>
        /// Фабрика создания подписей Microstation
        /// </summary>
        protected override ISignatureCreating SignatureCreating =>
            new SignatureCreatingMicrostation(this, StampSettings.Id, InsertSignatureByFields, SignaturesSearching,
                                              StampSettings.PersonId, StampSettings.UseDefaultSignature);

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> InsertSignatures() =>
            StampCellElement.ApplicationMicrostation.
            Map(application => application.AttachLibrary(StampCellElement.ApplicationMicrostation.MicrostationResources.SignatureMicrostationFileName)).
            ResultVoidOk(_ => DeleteSignaturesPrevious()).
            ResultValueOkBind(_ => InsertSignaturesFromLibrary()).
            ResultVoidOk(_ => StampCellElement.ApplicationMicrostation.DetachLibrary()).
            ToResultCollection();

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
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.
            Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted =>
                    new ResultAppCollection<IStampSignature>(signaturesDeleted,
                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                  "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private IResultAppCollection<IStampSignature> InsertSignaturesFromLibrary() =>
            StampSignatureFields.GetSignatures().
            ResultValueOkBind(InsertSignatures).
            ToResultCollection();


        /// <summary>
        /// Вставить подписи и получить поля
        /// </summary>
        private IResultAppCollection<IStampSignature> InsertSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.
            Select(SearchSignatureToInsert).
            ToResultCollection();

        /// <summary>
        /// Найти подпись в базе и вставить
        /// </summary>
        private IResultAppValue<IStampSignature> SearchSignatureToInsert(IStampSignature signature) =>
            SignaturesSearching.
            FindByIdOrFullNameOrRandom(signature.SignatureLibrary.PersonId,
                                       signature.SignatureLibrary.PersonInformation.FullName, StampSettings.PersonId).
            ResultValueOk(signatureLibrary => new SignatureFileApp(signatureLibrary.PersonId, signatureLibrary.PersonInformation,
                                                                   String.Empty, signature.IsVertical)).
            ResultValueOk(signature.InsertSignature);


        /// <summary>
        /// Вставить подпись между полями
        /// </summary>
        private IResultAppValue<ICellElementMicrostation> InsertSignatureByFields(string personId, ITextElementMicrostation previousField,
                                                                                  ITextElementMicrostation nextField) =>
            SignatureRange.GetSignatureRange(GetLinesStamp(), previousField, nextField, previousField.IsVertical).
            ResultValueOkBind(signatureRange => StampCellElement.ApplicationMicrostation.
                                                CreateCellElementFromLibrary(personId, signatureRange.OriginPoint,
                                                                             StampCellElement.ModelMicrostation,
                                                                             CreateSignatureCell(signatureRange, previousField.IsVertical)));

        /// <summary>
        /// Получить линии из штампа
        /// </summary>
        private IResultAppCollection<ILineElementMicrostation> GetLinesStamp() =>
            StampCellElement.GetSubElementsByType(ElementMicrostationType.LineElement).
            Select(element => element.AsLineElementMicrostation).
            Map(lineElements => new ResultAppCollection<ILineElementMicrostation>(lineElements));

        /// <summary>
        /// Параметры ячейки подписи
        /// </summary>
        private static Func<ICellElementMicrostation, ICellElementMicrostation> CreateSignatureCell(RangeMicrostation signatureRange,
                                                                                                    bool isVertical) =>
            cellElement => cellElement.Clone().
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
