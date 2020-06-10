using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampContainer;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampTypes;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

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
        /// Тип приложения
        /// </summary>
        public abstract StampApplicationType StampApplicationType { get; }

        /// <summary>
        /// Основные поля штампа
        /// </summary>
        private IStampBasicFields _stampBasicFields;

        /// <summary>
        /// Основные поля штампа
        /// </summary>
        public IStampBasicFields StampBasicFields => _stampBasicFields ??= GetStampBasicFields();

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        private IStampSignatureFields _stampSignatureFields;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        public IStampSignatureFields StampSignatureFields => _stampSignatureFields ??= GetStampSignatureFields();

        /// <summary>
        /// Тип документа, определяемый по типу шифра в штампе
        /// </summary>
        public StampDocumentType StampDocumentType =>
            FullCode.WhereContinue(fullCode => fullCode.OkStatus,
                okFunc: stamps => StampDocument.GetDocumentTypeByFullCode(FullCode.Value.Text, StampApplicationType),
                badFunc: _ => StampDocumentType.Unknown);

        /// <summary>
        /// Формат
        /// </summary>
        public abstract string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public abstract StampOrientationType Orientation { get; }

        /// <summary>
        /// Является ли тип штампа основным
        /// </summary>
        public bool IsStampTypeMain => StampTypeDefinition.IsStampTypeMain(StampType);

        /// <summary>
        /// Сжать поля
        /// </summary>
        public abstract IEnumerable<bool> CompressFieldsRanges();
    }
}
