﻿using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampFieldIndexes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки с изменениями
    /// </summary>
    public partial class SignatureCreatingWord
    {
        /// <summary>
        /// Получить строки с изменениями Word
        /// </summary>
        public override IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary) =>
             _stampFieldsWord.GetFieldsByType(StampFieldType.ChangeSignature).
            Select(field => _stampFieldsWord.GetTableRowByIndex(field.CellElementStamp.RowIndex, field.CellElementStamp.ColumnIndex,
                                               ChangeRowIndexes.NUMBER_CHANGE, ChangeSignatureWord.FIELDS_COUNT)).
            Where(row => row.CellsElement.Count >= ChangeSignatureWord.FIELDS_COUNT).
            Select(row => GetStampChangeFromRow(row, signatureLibrary, _stampIdentifier)).
            ToResultCollection().
            ResultValueOk(changeRows => changeRows.Where(ChangeSignatureValidation)).
            ToResultCollection();

        /// <summary>
        /// Получить класс с изменениями и подписью по строке Word
        /// </summary>
        private static IResultAppValue<IStampChange> GetStampChangeFromRow(IRowElementWord changeRow, ISignatureLibraryApp signatureLibrary,
                                                                           StampIdentifier stampIdentifier) =>
            new ResultAppValue<ISignatureLibraryApp>(signatureLibrary, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                            "Не найден идентификатор основной подписи")).
            ResultValueOk(signature => GetStampChangeFromFields(changeRow, signature, stampIdentifier));

        /// <summary>
        /// Получить класс с изменениями и подписью на основании полей Word
        /// </summary>
        private static IStampChange GetStampChangeFromFields(IRowElementWord changeRow, ISignatureLibraryApp personSignature,
                                                             StampIdentifier stampIdentifier) =>
            new ChangeSignatureWord(personSignature, stampIdentifier,
                                new StampFieldWord(changeRow.CellsElement[ChangeRowIndexes.SIGNATURE], StampFieldType.ChangeSignature),
                                new StampTextFieldWord(changeRow.CellsElement[ChangeRowIndexes.NUMBER_CHANGE], StampFieldType.ChangeSignature),
                                new StampTextFieldWord(changeRow.CellsElement[ChangeRowIndexes.NUMBER_PLOTS], StampFieldType.ChangeSignature),
                                new StampTextFieldWord(changeRow.CellsElement[ChangeRowIndexes.TYPE_OF_CHANGE], StampFieldType.ChangeSignature),
                                new StampTextFieldWord(changeRow.CellsElement[ChangeRowIndexes.DOCUMENT_CHANGE], StampFieldType.ChangeSignature),
                                new StampTextFieldWord(changeRow.CellsElement[ChangeRowIndexes.DATE_CHANGE], StampFieldType.ChangeSignature));
    }
}