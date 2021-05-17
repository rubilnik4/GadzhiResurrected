using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
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
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки согласования для извещения с изменениями Word
    /// </summary>
    public partial class SignatureCreatingWord
    {
        /// <summary>
        /// Получить строки с согласованием без подписи Word для извещения с изменениями
        /// </summary>
        public override IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows() =>
            _stampFieldsWord.GetFieldsByType(StampFieldType.ApprovalChangeSignature).
            Select(field => GetApprovalChangeTableRow(field.CellElementStamp.RowIndex, field.CellElementStamp.ColumnIndex)).
            Where(row => row.CellsElement.Count >= ApprovalChangeSignatureWord.FIELDS_COUNT).
            Select(GetStampApprovalChangeFromRow).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп подписей согласования извещения изменений не найден"));

        /// <summary>
        /// Получить строку, начиная от индекса маркера для штампа согласования
        /// </summary>
        private IRowElementWord GetApprovalChangeTableRow(int rowIndexField, int columnIndexField) =>
            Enumerable.Range(0, ApprovalChangeSignatureWord.FIELDS_COUNT).
            Select(index => rowIndexField - index).
            Where(rowIndex => _tableStamp.RowsElementWord.Count > rowIndex &&
                              _tableStamp.RowsElementWord[rowIndex].CellsElement.Count > columnIndexField).
            Select(rowIndex => rowIndex > 0
                       ? _tableStamp.RowsElementWord[rowIndex].CellsElement[columnIndexField]
                       : _tableStamp.RowsElementWord[rowIndex].CellsElement[columnIndexField - 1]).
            Map(cells => new RowElementWord(cells));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word для штампа согласования
        /// </summary>
        private IResultAppValue<IStampApprovalChange> GetStampApprovalChangeFromRow(IRowElementWord approvalChangeRow) =>
            CheckFieldType.GetDepartmentType(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.ACTION_TYPE].MaxLengthWord).
            Map(departmentType => GetSignatureInformation(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.RESPONSIBLE_PERSON].MaxLengthWord,
                                                          PersonId, departmentType,
                                                          approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.ACTION_TYPE].MaxLengthWord)).
            ResultValueOk(signature => GetStampApprovalChangeFromFields(approvalChangeRow, signature, _stampIdentifier));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word для штампа согласования
        /// </summary>
        private static IStampApprovalChange GetStampApprovalChangeFromFields(IRowElementWord approvalChangeRow, ISignatureLibraryApp approvalChangeSignature,
                                                                             StampIdentifier stampIdentifier) =>
            new ApprovalChangeSignatureWord(approvalChangeSignature, stampIdentifier,
                                            new StampFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.SIGNATURE], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.ACTION_TYPE], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.RESPONSIBLE_PERSON], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.DATE], StampFieldType.ApprovalChangeSignature));
    }
}