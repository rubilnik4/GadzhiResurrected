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
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки с ответственными лицами
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить строки с ответственным лицом без подписи Word
        /// </summary>
        protected override IResultAppCollection<IStampPerson> GetStampPersonRows() =>
            GetFieldsByType(StampFieldType.PersonSignature).
            Select(field => TableStamp.RowsElementWord[field.CellElementStamp.RowIndex]).
            Where(row => row.CellsElement.Count >= PersonSignatureWord.FIELDS_COUNT).
            Select(GetStampPersonFromRow).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп основных подписей не найден"));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonFromRow(IRowElementWord personRow) =>
            CheckFieldType.GetDepartmentType(personRow.CellsElement[PersonRowIndexes.ACTION_TYPE].Text).
            Map(departmentType => GetSignatureInformation(personRow.CellsElement[PersonRowIndexes.RESPONSIBLE_PERSON].Text, 
                                                          StampSettings.PersonId, departmentType)).
            ResultValueOk(signature => GetStampPersonFromFields(personRow, signature));

        /// <summary>
        /// Получить класс с ответственным лицом и подписью на основании полей Word
        /// </summary>
        private static IStampPerson GetStampPersonFromFields(IRowElementWord personRow, ISignatureLibraryApp personSignature) =>
            new PersonSignatureWord(personSignature,
                                new StampFieldWord(personRow.CellsElement[PersonRowIndexes.SIGNATURE], StampFieldType.PersonSignature),
                                new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.ACTION_TYPE], StampFieldType.PersonSignature),
                                new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.RESPONSIBLE_PERSON], StampFieldType.PersonSignature),
                                new StampTextFieldWord(personRow.CellsElement[PersonRowIndexes.DATE], StampFieldType.PersonSignature));
    }
}