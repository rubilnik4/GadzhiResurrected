using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Extensions.Collection;
using GadzhiMicrostation.Microstation.Implementations.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Основные поля штампа Word
    /// </summary>
    public class StampMainWord : StampWord, IStampMain<IStampFieldWord>
    {
        /// <summary>
        /// Таблица соответствия между фамилией и идентификатором с подписью
        /// </summary>
        private readonly IReadOnlyDictionary<string, (string PersonId, string SignaturePath)> _personIdTable;

        /// <summary>
        /// Строки с ответственным лицом и подписью Word
        /// </summary>
        public IResultAppCollection<IStampPersonWord> StampPersonsWord { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IResultAppCollection<IStampChangeWord> StampChangesWord { get; }

        public StampMainWord(ITableElement tableStamp, StampIdentifier id, string paperSize, OrientationType orientationType)
            : base(tableStamp, id, paperSize, orientationType)
        {
            _personIdTable = new Dictionary<string, (string, string)> {
                { "anyone", ("id", tableStamp?.ApplicationWord.WordResources.SignatureWordFileName) }
            };

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
            ResultValueOk(signatures => signatures.
                                        Select(signature => signature.InsertSignature(new List<LibraryElement>())).
                                        Cast<IStampSignature<IStampField>>().
                                        ToList()).
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
            Where(row => row.CellsElementWord?.Count >= StampPersonWord.FIELDS_COUNT).
            Select(row =>
                row.CellsElementWord[0].
                Map(personCell => new StampPersonWord(new StampFieldWord(personCell, StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                               GetSignatureInformationByPersonName(personCell.Text)))).
            Map(personRows => new ResultAppCollection<IStampPersonWord>(personRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                         "Штамп основных подписей не найден")));

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IResultAppCollection<IStampChangeWord> GetStampChangeSignatures(ISignatureInformation personInformation) =>
            FieldsStamp.Where(field => field.StampFieldType == StampFieldType.ChangeSignature).
            Select(field => field.CellElementStamp.RowElementWord).
            Where(row => row.CellsElementWord?.Count >= StampPersonWord.FIELDS_COUNT).
            Select(row => new StampChangeWord(new StampFieldWord(row.CellsElementWord[0], StampFieldType.PersonSignature),
                                              new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                              new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                              new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                              new StampFieldWord(row.CellsElementWord[4], StampFieldType.PersonSignature),
                                              new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                              personInformation)).
            Map(changeRows => new ResultAppCollection<IStampChangeWord>(changeRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                         "Штамп подписей замены не найден")));


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
        private SignatureInformation GetSignatureInformationByPersonName(string personName) =>
            _personIdTable.FirstOrDefault().Value.
            Map(personId => new SignatureInformation(personName, personId.PersonId, personId.SignaturePath));
    }
}
