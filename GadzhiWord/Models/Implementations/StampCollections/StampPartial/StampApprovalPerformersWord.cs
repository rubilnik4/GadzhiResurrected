using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampFieldIndexes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки согласования со списком исполнителей Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить строки с согласованием со списком исполнителей без подписи Word для извещения с изменениями
        /// </summary>
        protected override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            TableApprovalPerformers.
            ResultValueOk(table => table.RowsElementWord).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.FieldNotFound, "Строки согласования не найдены")).
            ResultValueOk(rows => rows.Where(row => row.CellsElement.Count == ApprovalPerformersSignatureWord.FIELDS_COUNT)).
            ResultValueOkBind(rows => rows.Select(GetStampApprovalPerformersFromRow).
                                      ToResultCollection()).
            ToResultCollection();

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word для строк согласования
        /// </summary>
        private IResultAppValue<IStampApprovalPerformers> GetStampApprovalPerformersFromRow(IRowElementWord approvalPerformersRow) =>
            GetSignatureInformation(approvalPerformersRow.CellsElement[ApprovalChangeRowIndexes.RESPONSIBLE_PERSON].Text,
                                                          StampSettings.PersonId, PersonDepartmentType.Undefined).
            ResultValueOk(signature => GetStampApprovalPerformanceFromFields(approvalPerformersRow, signature));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word  для строк согласования
        /// </summary>
        private static IStampApprovalPerformers GetStampApprovalPerformanceFromFields(IRowElementWord approvalPerformersRow,
                                                                                      ISignatureLibraryApp approvalChangeSignature) =>
            new ApprovalPerformersSignatureWord(approvalChangeSignature,
                                            new StampFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.SIGNATURE], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.RESPONSIBLE_PERSON], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DEPARTMENT], StampFieldType.ApprovalPerformersSignature),
                                            new StampTextFieldWord(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DATE], StampFieldType.ApprovalPerformersSignature));
    }
}