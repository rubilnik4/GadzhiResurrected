using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Extensions.Functional.Result;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Основные поля штампа Word
    /// </summary>
    public class StampMainWord : StampWord, IStampMain<IStampFieldWord>
    {
        /// <summary>
        /// Строки с ответственным лицом и подписью Word
        /// </summary>
        public IResultAppCollection<IStampPersonWord> StampPersonsWord { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IResultAppCollection<IStampChangeWord> StampChangesWord { get; }

        public StampMainWord(ITableElement tableStamp, StampIdentifier id, string paperSize, OrientationType orientationType,
                             SignaturesLibrarySearching signaturesLibrarySearching)
            : base(tableStamp, id, paperSize, orientationType, signaturesLibrarySearching)
        {
            StampPersonsWord = GetStampPersonSignatures();
            StampChangesWord = GetStampChangeSignatures(StampPersonsWord.Value?.FirstOrDefault());
        }

        /// <summary>
        /// Строки с ответственным лицом и подписью Word
        /// </summary>
        public IResultAppCollection<IStampPerson<IStampFieldWord>> StampPersons =>
            new ResultAppCollection<IStampPerson<IStampFieldWord>>(StampPersonsWord.Value, StampPersonsWord.Errors);
        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IResultAppCollection<IStampChange<IStampFieldWord>> StampChanges =>
            new ResultAppCollection<IStampChange<IStampFieldWord>>(StampChangesWord.Value, StampChangesWord.Errors);

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature<IStampField>> InsertSignatures() =>
            GetSignatures(StampPersonsWord, StampChangesWord).
            ResultValueOkBind(GetStampSignaturesByIds).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature<IStampField>> DeleteSignatures(IEnumerable<IStampSignature<IStampField>> signatures) =>
            signatures.
            Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted => new ResultAppCollection<IStampSignature<IStampField>>
                                     (signaturesDeleted,
                                      signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                                      new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IResultAppCollection<IStampPersonWord> GetStampPersonSignatures() =>
            FieldsStamp.
            Where(field => field.StampFieldType == StampFieldType.PersonSignature).
            Select(field => field.CellElementStamp.RowElementWord).
            Where(row => row.CellsElementWord.Count >= StampPersonWord.FIELDS_COUNT).
            Select(GetStampPersonWordByRow).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп основных подписей не найден"));

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IResultAppCollection<IStampChangeWord> GetStampChangeSignatures(ISignatureLibrary signatureLibrary) =>
            new ResultAppValue<ISignatureLibrary>(signatureLibrary, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                         "Не найден идентификатор основной подписи")).
            ResultValueOk(_ => FieldsStamp.Where(field => field.StampFieldType == StampFieldType.ChangeSignature).
                                           Select(field => field.CellElementStamp.RowElementWord).
                                           Where(row => row.CellsElementWord.Count >= StampPersonWord.FIELDS_COUNT).
                                           Select(row => GetStampChangeWordByRow(row, signatureLibrary))).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп подписей замены не найден"));

        /// <summary>
        /// Получить подписи
        /// </summary>        
        private static IResultAppCollection<IStampSignature<IStampFieldWord>> GetSignatures(IResultAppCollection<IStampPersonWord> personSignatures,
                                                                                            IResultAppCollection<IStampChangeWord> changeSignatures) =>
             personSignatures.Cast<IStampPersonWord, IStampSignature<IStampFieldWord>>().
                              ConcatValues(changeSignatures.Value);

        /// <summary>
        /// Получить информацию об ответственном лице по имени
        /// </summary>      
        private IResultAppValue<ISignatureLibrary> GetSignatureInformationByPersonName(string personName) =>
            SignaturesLibrarySearching.FindByFullNameOrRandom(personName).
            ResultValueOk(signature => new SignatureLibrary(signature.PersonId, signature.PersonName));

        /// <summary>
        /// Получить элементы подписей из базы по их идентификационным номерам
        /// </summary>
        private IResultAppCollection<IStampSignature<IStampField>> GetStampSignaturesByIds(IList<IStampSignature<IStampFieldWord>> signaturesStamp) =>
            new ResultAppCollection<string>(signaturesStamp.Select(signatureStamp => signatureStamp.PersonId)).
            ResultValueOkBind(personIds => SignaturesLibrarySearching.GetSignaturesByIds(personIds)).
            ResultValueContinue(signaturesFile => signaturesFile.Count == signaturesStamp.Count,
                okFunc: signaturesFile => signaturesFile,
                badFunc: signaturesFile => new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                "Количество подписей в файле не совпадает с загруженным из базы данных")).
            ResultValueOk(signaturesFile =>
                signaturesStamp.Join(signaturesFile,
                                     signatureStamp => signatureStamp.PersonId,
                                     signatureFile => signatureFile.PersonId,
                                     (signatureStamp, signatureFile) => (IStampSignature<IStampField>)signatureStamp.InsertSignature(signatureFile))).
            ToResultCollection();

        /// <summary>
        /// Получить класс с ответственным лицом и подписью по строке Word
        /// </summary>
        private IResultAppValue<IStampPersonWord> GetStampPersonWordByRow(IRowElement personRow) =>
            GetSignatureInformationByPersonName(personRow.CellsElementWord[1].Text).
            ResultValueOk(signature => new StampPersonWord(new StampFieldWord(personRow.CellsElementWord[0], StampFieldType.PersonSignature),
                                                           new StampFieldWord(personRow.CellsElementWord[1], StampFieldType.PersonSignature),
                                                           new StampFieldWord(personRow.CellsElementWord[2], StampFieldType.PersonSignature),
                                                           new StampFieldWord(personRow.CellsElementWord[3], StampFieldType.PersonSignature),
                                                           signature));

        /// <summary>
        /// Получить класс с изменениями и подписью по строке Word
        /// </summary>
        private static IStampChangeWord GetStampChangeWordByRow(IRowElement changeRow, ISignatureLibrary signatureLibrary) =>
            new StampChangeWord(new StampFieldWord(changeRow.CellsElementWord[0], StampFieldType.PersonSignature),
                                new StampFieldWord(changeRow.CellsElementWord[1], StampFieldType.PersonSignature),
                                new StampFieldWord(changeRow.CellsElementWord[2], StampFieldType.PersonSignature),
                                new StampFieldWord(changeRow.CellsElementWord[3], StampFieldType.PersonSignature),
                                new StampFieldWord(changeRow.CellsElementWord[4], StampFieldType.PersonSignature),
                                new StampFieldWord(changeRow.CellsElementWord[3], StampFieldType.PersonSignature),
                                signatureLibrary);
    }
}
