using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Models.Enums;
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
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract class Stamp : IStamp
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        protected ITableElementWord TableStamp { get; }

        public Stamp(ITableElementWord tableStamp)
        {
            TableStamp = tableStamp;
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name => $"{StampAdditionalParameters.StampTypeToString[StampType]}";
      
        /// <summary>
        /// Тип штампа
        /// </summary>
        protected abstract StampType StampType { get; }       

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
