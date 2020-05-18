using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;


namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public interface IStamp
    {
        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        public StampIdentifier Id { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        OrientationType Orientation { get; }

        /// <summary>
        /// Сжать поля
        /// </summary>
        IEnumerable<bool> CompressFieldsRanges();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        IResultAppCollection<IStampSignature<IStampField>> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        IResultAppCollection<IStampSignature<IStampField>> DeleteSignatures(IEnumerable<IStampSignature<IStampField>> signatures);
    }
}
