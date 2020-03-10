using ConvertingModels.Models.Enums;
using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiConverting.Word.Interfaces.Elements;
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
    public class StampMain : Stamp, IStampMain
    {
        public StampMain(ITableElement tableStamp, IDocumentLibrary documentWord)
            : base(tableStamp, documentWord)
        {

        }
              
        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;
        
        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        public IEnumerable<IStampPersonSignature> StampPersonSignatures => FieldsStamp.Where(field => field.StampFieldType == StampFieldType.PersonSignature).
                                                                                       Select(field => new StampPersonSignature(field.RowElementStamp));     
    }
}
