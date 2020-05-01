using GadzhiApplicationCommon.Extensions.Collection;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Extentions.Collection;
using GadzhiMicrostation.Models.Enums;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IReadOnlyCollection<IStampPersonSignatureWord> StampPersonSignaturesWord { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IReadOnlyCollection<IStampChangeSignatureWord> StampChangeSignaturesWord { get; }

        public StampMainWord(ITableElement tableStamp, string paperSize, OrientationType orientationType)
            : base(tableStamp, paperSize, orientationType)
        {
            _personIdTable = new Dictionary<string, (string, string)> {
                { "anyone", ("id", tableStamp?.ApplicationWord.WordResources.SignatureWordFileName) }
            };           

            StampPersonSignaturesWord = GetStampPersonSignatures().ToList();
            StampChangeSignaturesWord = GetStampChangeSignatures(StampPersonSignaturesWord.FirstOrDefault()).ToList();
        }

        /// <summary>
        /// Строки с ответственным лицом и подписью Word
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldWord>> StampPersonSignatures => StampPersonSignaturesWord;

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IEnumerable<IStampChangeSignature<IStampFieldWord>> StampChangeSignatures => StampChangeSignaturesWord;

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultApplication InsertSignatures() =>
            GetSignatures(StampPersonSignatures, StampChangeSignatures).
            Select(signature => signature.InsertSignature()).
            Where(signature => !signature.IsSignatureValid).
            Select(signature => new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Не найдена подпись {signature.PersonName}")).
            ToList().
            ToResultApplication();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IEnumerable<Unit> DeleteSignatures() =>
            GetSignatures(StampPersonSignatures, StampChangeSignatures).
            Where(signature => signature.IsSignatureValid).
            Select(signature => { signature.DeleteSignature(); return Unit.Value; });

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampPersonSignatureWord> GetStampPersonSignatures() =>
            FieldsStamp.
            Where(field => field.StampFieldType == StampFieldType.PersonSignature).
            Select(field => field.CellElementStamp.RowElementWord).
            Where(row => row.CellsElementWord?.Count >= StampPersonSignatureWord.FieldsCount).
            Select(row =>
                row.CellsElementWord[0].
                Map(personCell => new StampPersonSignatureWord(new StampFieldWord(personCell, StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                                               new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                               GetSignatureInformationByPersonName(personCell.Text))));

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IEnumerable<IStampChangeSignatureWord> GetStampChangeSignatures(ISignatureInformation personInformation) =>
            FieldsStamp.Where(field => field.StampFieldType == StampFieldType.ChangeSignature).
            Select(field => field.CellElementStamp.RowElementWord).
            Where(row => row.CellsElementWord?.Count >= StampPersonSignatureWord.FieldsCount).
            Select(row => new StampChangeSignatureWord(new StampFieldWord(row.CellsElementWord[0], StampFieldType.PersonSignature),
                                                       new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                                       new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                                       new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                       new StampFieldWord(row.CellsElementWord[4], StampFieldType.PersonSignature),
                                                       new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                       personInformation));

        /// <summary>
        /// Получить подписи
        /// </summary>        
        private static IEnumerable<IStampSignature<IStampFieldWord>> GetSignatures(IEnumerable<IStampSignature<IStampFieldWord>> personSignatures,
                                                                                   IEnumerable<IStampSignature<IStampFieldWord>> changeSignatures) =>
             personSignatures.UnionNotNull(changeSignatures);

        /// <summary>
        /// Получить информацию об ответственном лице по имени
        /// </summary>      
        private SignatureInformation GetSignatureInformationByPersonName(string personName) =>
            _personIdTable.FirstOrDefault().Value.
            Map(personId => new SignatureInformation(personName, personId.PersonId, personId.SignaturePath));
    }
}
