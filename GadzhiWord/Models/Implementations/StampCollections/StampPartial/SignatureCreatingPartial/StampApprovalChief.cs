using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampDefinitions;
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
    /// Строки согласования без подписи для опросных листов и тех требований с директорами
    /// </summary>
    public partial class SignatureCreatingWord
    {
        /// <summary>
        /// Получить строки согласования без подписи для опросных листов и тех требований с директорами
        /// </summary>
        public override IResultAppCollection<IStampApprovalChief> GetStampApprovalChiefRows() =>
           StampDocumentDefinition.IsDocumentQuestionnaire(_stampDocumentType)
               ? GetStampApprovalChiefRowsChecked()
               : new ResultAppCollection<IStampApprovalChief>(Enumerable.Empty<IStampApprovalChief>());

        /// <summary>
        /// Получить строки с согласованием с директорами для опросных листов и технических требований
        /// </summary>
        private IResultAppCollection<IStampApprovalChief> GetStampApprovalChiefRowsChecked() =>
            _tableApprovalChief.
            ResultValueOk(table => table.RowsElementWord).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.FieldNotFound, "Строки согласования с директорами не найдены")).
            ResultValueOk(rows => rows.Where(row => row.CellsElement.Count == ApprovalChiefSignatureWord.FIELDS_COUNT)).
            ResultValueOkBind(rows => rows.
                                      Where(row => ConverterDepartmentTypeApp.HasDepartmentType(row.CellsElement[ApprovalChiefRowIndexes.DEPARTMENT].Text)).
                                      Select(GetStampApprovalChiefFromRow).
                                      ToResultCollection()).
            ToResultCollection();

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word для строк согласования тех требований с директорами
        /// </summary>
        private IResultAppValue<IStampApprovalChief> GetStampApprovalChiefFromRow(IRowElementWord approvalChiefRow) =>
            ConverterDepartmentTypeApp.DepartmentParsing(approvalChiefRow.CellsElement[ApprovalChiefRowIndexes.DEPARTMENT].Text).
            Map(departmentType => SignaturesSearching.FindByFullNameOrRandom(approvalChiefRow.CellsElement[ApprovalChiefRowIndexes.RESPONSIBLE_PERSON].MaxLengthWord,
                                                                             departmentType)).
            ResultValueOk(signature => GetStampApprovalChiefFromFields(approvalChiefRow, signature));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word для строк согласования тех требований с директорами
        /// </summary>
        private static IStampApprovalChief GetStampApprovalChiefFromFields(IRowElementWord approvalChiefRow,
                                                                           ISignatureLibraryApp approvalChiefSignature) =>
            new ApprovalChiefSignatureWord(approvalChiefSignature,
                                            new StampFieldWord(approvalChiefRow.CellsElement[ApprovalChiefRowIndexes.SIGNATURE], StampFieldType.ApprovalChiefSignature),
                                            new StampTextFieldWord(approvalChiefRow.CellsElement[ApprovalChiefRowIndexes.RESPONSIBLE_PERSON], StampFieldType.ApprovalChiefSignature),
                                            new StampTextFieldWord(approvalChiefRow.CellsElement[ApprovalChiefRowIndexes.DEPARTMENT], StampFieldType.ApprovalChiefSignature));
    }
}