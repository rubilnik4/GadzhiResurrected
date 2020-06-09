using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Models.Implementations.StampFieldIndexes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля основного штампа Word
    /// </summary>
    public class StampFullWord : StampWord, IStampFull
    {
        public StampFullWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching,
                             ITableElementWord tableStamp, IResultAppValue<ITableElementWord> tableApprovalPerformers)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            _tableApprovalPerformers = tableApprovalPerformers ?? throw new ArgumentNullException(nameof(tableApprovalPerformers));
        }

        /// <summary>
        /// Элемент таблица согласования списка исполнителей
        /// </summary>
        private readonly IResultAppValue<ITableElementWord> _tableApprovalPerformers;

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Full;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows, GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary)));

        /// <summary>
        /// Получить строки с согласованием без подписи Word для извещения с изменениями
        /// </summary>
        protected override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            _tableApprovalPerformers.
            ResultValueOk(table => table.RowsElementWord).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.FieldNotFound, "Строки согласования не найдены")).
            ResultValueOk(rows => rows.Where(row => row.CellsElement.Count == ApprovalPerformersSignatureWord.FIELDS_COUNT)).
            ResultValueOkBind(rows => rows.Select(GetStampApprovalPerformersFromRow).
                                      ToResultCollection()).
            ToResultCollection();

         /// <summary>
         /// Получить класс с ответственным лицом и подписью по строке Word для штампа согласования
         /// </summary>
        private IResultAppValue<IStampApprovalPerformers> GetStampApprovalPerformersFromRow(IRowElementWord approvalPerformersRow) =>
            CheckFieldType.GetDepartmentType(approvalPerformersRow.CellsElement[ApprovalPerformersRowIndexes.DEPARTMENT].Text).
            Map(departmentType => GetSignatureInformation(approvalPerformersRow.CellsElement[ApprovalChangeRowIndexes.RESPONSIBLE_PERSON].Text,
                                                          StampSettings.PersonId, departmentType)).
            ResultValueOk(signature => GetStampApprovalPerformanceFromFields(approvalPerformersRow, signature));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word для штампа согласования
        /// </summary>
        private static IStampApprovalChange GetStampApprovalChangeFromFields(IRowElementWord approvalChangeRow, ISignatureLibraryApp approvalChangeSignature) =>
            new ApprovalChangeSignatureWord(approvalChangeSignature,
                                            new StampFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.SIGNATURE], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.ACTION_TYPE], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.RESPONSIBLE_PERSON], StampFieldType.ApprovalChangeSignature),
                                            new StampTextFieldWord(approvalChangeRow.CellsElement[ApprovalChangeRowIndexes.DATE], StampFieldType.ApprovalChangeSignature));
    }
}
