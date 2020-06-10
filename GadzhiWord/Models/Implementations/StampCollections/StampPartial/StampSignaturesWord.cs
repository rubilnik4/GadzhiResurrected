using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> InsertSignatures() =>
            StampSignatureFields.GetSignatures().
            ResultValueOkBind(GetStampSignaturesByIds).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.
            Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted => new ResultAppCollection<IStampSignature>
                                     (signaturesDeleted,
                                      signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                                      new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Получить элементы подписей из базы по их идентификационным номерам
        /// </summary>
        private IResultAppCollection<IStampSignature> GetStampSignaturesByIds(IList<IStampSignature> stampSignatures) =>
            new ResultAppCollection<IStampSignature>(stampSignatures).
            ResultValueOk(_ => stampSignatures.Select(signatureStamp => new SignatureFileRequest(signatureStamp.SignatureLibrary.PersonId,
                                                                                                 signatureStamp.IsVertical))).
            ResultValueOkBind(personIds => SignaturesSearching.GetSignaturesByIds(personIds)).
            ResultValueContinue(signaturesFile => signaturesFile.Count == stampSignatures.Count,
                okFunc: signaturesFile => signaturesFile,
                badFunc: signaturesFile => new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                "Количество подписей в файле не совпадает с загруженным из базы данных")).
            ResultValueOk(signaturesFile =>
                stampSignatures.Zip(signaturesFile,
                                    (signatureStamp, signatureFile) => signatureStamp.InsertSignature(signatureFile))).
            ToResultCollection();

        /// <summary>
        /// Получить информацию об ответственном лице по имени
        /// </summary>      
        private IResultAppValue<ISignatureLibraryApp> GetSignatureInformation(string personName, string personId,
                                                                              PersonDepartmentType personDepartmentType) =>
            SignaturesSearching.FindById(personId)?.PersonInformation.DepartmentType.
            Map(departmentType => SignaturesSearching.CheckDepartmentAccordingToType(departmentType, personDepartmentType)).
            Map(departmentChecked => SignaturesSearching.FindByFullNameOrRandom(personName, departmentChecked));

        /// <summary>
        /// Получить строку, начиная от индекса маркера
        /// </summary>
        private IRowElementWord GetTableRowByIndex(int rowIndex, int columnStartIndex, int indexColumnFirst, int fieldsCount) =>
            Enumerable.Range(indexColumnFirst, fieldsCount).
                       Where(indexColumn => TableStamp.RowsElementWord[rowIndex].CellsElement.Count > columnStartIndex + indexColumn).
                       Select(indexColumn => TableStamp.RowsElementWord[rowIndex].CellsElement[columnStartIndex + indexColumn]).
                       Map(cells => new RowElementWord(cells));
    }
}