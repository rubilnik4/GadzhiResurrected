using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiConverting.Word.Interfaces.Elements;
using ConvertingModels.Models.Enums;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract class Stamp : IStamp
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        protected ITableElement TableStamp { get; }

        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        private readonly IDocumentWord _documentWord;

        public Stamp(ITableElement tableStamp, IDocumentWord documentWord)
        {
            TableStamp = tableStamp;
            _documentWord = documentWord;
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
        public string PaperSize => _documentWord.PaperSize;

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IEnumerable<IStampField> FieldsStamp => GetFields();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IEnumerable<IStampField> GetFields() =>
            TableStamp?.CellsElementWord?.Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
                                          Select(cell => new StampField(cell)).
                                          Where(field => field.StampFieldType != StampFieldType.Unknown);


    }
}
