using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces;
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
        /// Строки с ответсвенным лицом и подписью Word
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldWord>> StampPersonSignatures { get; }

        /// <summary>
        /// Строки с изменениями Word
        /// </summary>
        public IEnumerable<IStampChangeSignature<IStampFieldWord>> StampChangeSignatures { get; }

        public StampMainWord(ITableElement tableStamp, string paperSize, OrientationType orientationType)
            : base(tableStamp, paperSize, orientationType)
        {
            StampPersonSignatures = GetStampPersonSignatures();
            StampChangeSignatures = GetStampChangeSignatures();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override void InsertSignatures()
        {
            foreach (var personSignature in StampPersonSignatures)
            {
                personSignature.Signature.CellElementStamp.DeleteAllPictures();
                personSignature.Signature.CellElementStamp.
                                InsertPicture(TableStamp.ApplicationWord.WordResources.SignatureWordFileName);
            }
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override void DeleteSignatures()
        {
            foreach (var personSignature in StampPersonSignatures)
            {
                personSignature.Signature.CellElementStamp.DeleteAllPictures();
            }
        }

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampPersonSignature<IStampFieldWord>> GetStampPersonSignatures() =>
              FieldsStamp.Where(field => field.StampFieldType == StampFieldType.PersonSignature).
                          Select(field => field.CellElementStamp.RowElementWord).
                          Where(row => row.CellsElementWord?.Count >= StampPersonSignatureWord.FieldsCount).
                          Select(row => new StampPersonSignatureWord(new StampFieldWord(row.CellsElementWord[0], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature)));

        /// <summary>
        /// Получить cтроки с изменениями
        /// </summary>
        private IEnumerable<IStampChangeSignature<IStampFieldWord>> GetStampChangeSignatures() =>
              FieldsStamp.Where(field => field.StampFieldType == StampFieldType.ChangeSignature).
                          Select(field => field.CellElementStamp.RowElementWord).
                          Where(row => row.CellsElementWord?.Count >= StampPersonSignatureWord.FieldsCount).
                          Select(row => new StampChangeSignatureWord(new StampFieldWord(row.CellsElementWord[0], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[1], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[2], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[4], StampFieldType.PersonSignature),
                                                                     new StampFieldWord(row.CellsElementWord[3], StampFieldType.PersonSignature),
                                                                     StampPersonSignatures?.FirstOrDefault().AttributePersonId));
    }
}
