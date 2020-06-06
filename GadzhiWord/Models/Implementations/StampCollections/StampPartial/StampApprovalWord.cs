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
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки с согласованием
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить строки с согласованием без подписи Word
        /// </summary>
        protected override IResultAppCollection<IStampApproval> GetStampApprovalRows() =>
            GetFieldsByType(StampFieldType.ApprovalSignature).
            Select(field => GetApprovalTableRow(field.CellElementStamp.RowIndex, field.CellElementStamp.ColumnIndex)).
            Where(row => row.CellsElement.Count >= ApprovalSignatureWord.FIELDS_COUNT).
            Select(GetStampApprovalFromRow).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп подписей согласования не найден"));

        /// <summary>
        /// Получить строку, начиная от индекса маркера для штампа согласования
        /// </summary>
        private IRowElementWord GetApprovalTableRow(int rowIndexField, int columnIndexField) =>
            Enumerable.Range(0, ApprovalSignatureWord.FIELDS_COUNT).
            Select(index => rowIndexField - index).
            Where(rowIndex => TableStamp.RowsElementWord.Count > rowIndex &&
                           TableStamp.RowsElementWord[rowIndex].CellsElement.Count > columnIndexField).
            Select(rowIndex => rowIndex > 0
                       ? TableStamp.RowsElementWord[rowIndex].CellsElement[columnIndexField]
                       : TableStamp.RowsElementWord[rowIndex].CellsElement[columnIndexField - 1]).
            Map(cells => new RowElementWord(cells));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word
        /// </summary>
        private IResultAppValue<IStampApproval> GetStampApprovalFromRow(IRowElementWord approvalRow) =>
            CheckFieldType.GetDepartmentType(approvalRow.CellsElement[PersonRowIndexes.ACTION_TYPE].Text).
            Map(departmentType => GetSignatureInformation(approvalRow.CellsElement[PersonRowIndexes.RESPONSIBLE_PERSON].Text,
                                                          StampSettings.PersonId, departmentType)).
            ResultValueOk(signature => GetStampApprovalFromFields(approvalRow, signature));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word
        /// </summary>
        private static IStampApproval GetStampApprovalFromFields(IRowElementWord approvalRow, ISignatureLibraryApp approvalSignature) =>
            new PersonSignatureWord(personSignature,
                                    new StampFieldWord(personRow.CellsElement[PersonRowIndexes.SIGNATURE], StampFieldType.PersonSignature),
                                    new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.ACTION_TYPE], StampFieldType.PersonSignature),
                                    new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.RESPONSIBLE_PERSON], StampFieldType.PersonSignature),
                                    new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.DATE], StampFieldType.PersonSignature));
    }
}