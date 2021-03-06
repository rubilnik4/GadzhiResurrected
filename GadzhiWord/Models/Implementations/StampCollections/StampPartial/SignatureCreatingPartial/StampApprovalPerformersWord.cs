﻿using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampFieldIndexes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки согласования со списком исполнителей Word
    /// </summary>
    public partial class SignatureCreatingWord
    {
        /// <summary>
        /// Получить строки с согласованием со списком исполнителей без подписи Word для извещения с изменениями
        /// </summary>
        public override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            (_stampDocumentType == StampDocumentType.TechnicalRequirements || _stampDocumentType == StampDocumentType.Questionnaire)
                ? GetStampApprovalPerformersRowsChecked()
                : new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>());

        /// <summary>
        /// Получить строки с согласованием для опросных листов и технических требований
        /// </summary>
        private IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRowsChecked() =>
            _tableApprovalPerformers.
            ResultValueOk(table => table.RowsElementWord).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.FieldNotFound, "Строки согласования тех требований не найдены")).
            ResultValueOk(rows => rows.Where(row => row.CellsElement.Count == ApprovalPerformersSignatureWord.FIELDS_COUNT)).
            ResultValueOkBind(rows => rows.
                                      Where(row => ConverterDepartmentTypeApp.HasDepartmentType(row.CellsElement[ApprovalPerformersRowIndexes.DEPARTMENT].Text)).
                                      Select(GetStampApprovalPerformersFromRow).
                                      ToResultCollection()).
            ResultValueBad(rows => new List<IStampApprovalPerformers>()).
            ToResultCollection();

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word для строк согласования тех требований
        /// </summary>
        private IResultAppValue<IStampApprovalPerformers> GetStampApprovalPerformersFromRow(IRowElementWord approvalPerformersRow) =>
            ConverterDepartmentTypeApp.DepartmentParsing(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DEPARTMENT].Text).
            Map(departmentType => SignaturesSearching.FindByFullNameOrRandom(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.RESPONSIBLE_PERSON].MaxLengthWord,
                                                                             departmentType)).
            ResultValueOk(signature => GetStampApprovalPerformanceFromFields(approvalPerformersRow, signature, _stampIdentifier));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word для строк согласования тех требований
        /// </summary>
        private static IStampApprovalPerformers GetStampApprovalPerformanceFromFields(IRowElementWord approvalPerformersRow,
                                                                                      ISignatureLibraryApp approvalPerformersSignature,
                                                                                      StampIdentifier stampIdentifier) =>
            new ApprovalPerformersSignatureWord(approvalPerformersSignature, stampIdentifier,
                                            new StampFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.SIGNATURE], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.RESPONSIBLE_PERSON], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DEPARTMENT], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DATE], StampFieldType.ApprovalPerformersSignature));
    }
}