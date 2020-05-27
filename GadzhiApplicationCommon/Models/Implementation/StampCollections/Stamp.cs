using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Штамп. Базовый класс
    /// </summary>
    public abstract class Stamp : IStamp
    {
        protected Stamp(StampSettings stampSettings)
        {
            StampSettings = stampSettings ?? throw new ArgumentNullException(nameof(stampSettings));
        }

        /// <summary>
        /// Параметры штампа
        /// </summary>
        public StampSettings StampSettings { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public abstract StampType StampType { get; }

        /// <summary>
        /// Основные поля штампа
        /// </summary>
        public abstract IStampBasicFields StampMainFields { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public abstract string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public abstract StampOrientationType Orientation { get; }

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
