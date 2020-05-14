using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Штамп. Базовый класс
    /// </summary>
    public abstract class Stamp : IStamp
    {
        protected Stamp(StampIdentifier id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        public StampIdentifier Id { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public abstract StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public abstract string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public abstract OrientationType Orientation { get; }

        /// <summary>
        /// Сжать поля
        /// </summary>
        public abstract IEnumerable<bool> CompressFieldsRanges();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature<IStampField>> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature<IStampField>> DeleteSignatures(IEnumerable<IStampSignature<IStampField>> signatures);
    }
}
