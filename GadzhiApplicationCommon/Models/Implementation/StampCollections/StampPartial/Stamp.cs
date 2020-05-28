using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый класс
    /// </summary>
    public abstract partial class Stamp : IStamp
    {
        protected Stamp(StampSettings stampSettings, SignaturesSearching signaturesSearching)
        {
            StampSettings = stampSettings ?? throw new ArgumentNullException(nameof(stampSettings));
            SignaturesSearching = signaturesSearching ?? throw new ArgumentNullException(nameof(signaturesSearching));
        }

        /// <summary>
        /// Параметры штампа
        /// </summary>
        public StampSettings StampSettings { get; }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        protected SignaturesSearching SignaturesSearching { get; }

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
        private IResultAppValue<IStampBasicFields> _stampBasicFields;

        /// <summary>
        /// Основные поля штампа
        /// </summary>
        public IResultAppValue<IStampBasicFields> StampBasicFields => _stampBasicFields ??= GetStampBasicFields();

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
    }
}
