using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
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
    /// Основные поля штампа
    /// </summary>
    public class StampMainWord : StampWord, IStampMain<IStampFieldWord>
    {
        public StampMainWord(ITableElement tableStamp, string paperSize, OrientationType orientationType)
            : base(tableStamp, paperSize, orientationType)
        {
            StampPersonSignatures = GetStampPersonWithoutSignatures();
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature<IStampFieldWord>> StampPersonSignatures { get; }
              

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override void InsertSignatures()
        {
            foreach (var personSignature in StampPersonSignatures)
            {
                personSignature.Signature.CellElementStamp.DeleteAllPictures();
                personSignature.Signature.CellElementStamp.InsertPicture("signature.jpg");
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
        private IEnumerable<IStampPersonSignature<IStampFieldWord>>  GetStampPersonWithoutSignatures() =>
              FieldsStamp.Where(field => field.StampFieldType == StampFieldType.PersonSignature).
                            Select(field => new StampPersonSignatureWord(field.CellElementStamp.RowElementWord));
    }
}
