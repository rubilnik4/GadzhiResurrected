using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Interfaces.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract class Stamp : IStamp
    {
        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElement TableStamp { get; }
       
        public Stamp(ITableElement tableStamp, string paperSize, OrientationType orientationType)
        {
            TableStamp = tableStamp;
            PaperSize = paperSize;
            Orientation = orientationType;
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name => $"{StampAdditionalParameters.StampTypeToString[StampType]}";

        /// <summary>
        /// Тип штампа
        /// </summary>
        public abstract StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public OrientationType Orientation { get; }

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IEnumerable<IStampFieldWord> FieldsStamp => GetFields();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IEnumerable<IStampFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
                                      Select(cell => new StampField(cell)).
                                      Where(field => field.StampFieldType != StampFieldType.Unknown);


    }
}
