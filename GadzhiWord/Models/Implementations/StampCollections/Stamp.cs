using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;

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

        /// <summary>
        /// Документ
        /// </summary>
        private readonly IDocumentLibrary _document;

        public Stamp(ITableElement tableStamp, IDocumentLibrary document)
        {
            TableStamp = tableStamp;
            _document = document;
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
        public string PaperSize => _document.PaperSize;

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IEnumerable<IStampField> FieldsStamp => GetFields();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IEnumerable<IStampField> GetFields() =>
            TableStamp?.CellsElement?.Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
                                          Select(cell => new StampField(cell)).
                                          Where(field => field.StampFieldType != StampFieldType.Unknown);


    }
}
