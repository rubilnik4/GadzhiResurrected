using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract partial class StampWord : Stamp
    {
        protected StampWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching,
                            ITableElementWord tableStamp)
            : base(stampSettingsWord, signaturesSearching)
        {
            TableStamp = tableStamp;
            PaperSize = stampSettingsWord.PaperSize;
            Orientation = stampSettingsWord.Orientation;
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElementWord TableStamp { get; }

        /// <summary>
        /// Элемент таблица согласования списка исполнителей тех требований
        /// </summary>
        protected virtual IResultAppValue<ITableElementWord> TableApprovalPerformers =>
            new ResultAppValue<ITableElementWord>(new ErrorApplication(ErrorApplicationType.TableNotFound,
                                                                       "Таблица согласования списка исполнителей тех требований не найдена"));

        /// <summary>
        /// Элемент таблица согласования тех требований с директорами
        /// </summary>
        protected virtual IResultAppValue<ITableElementWord> TableApprovalChief =>
            new ResultAppValue<ITableElementWord>(new ErrorApplication(ErrorApplicationType.TableNotFound, 
                                                                       "Таблица согласования тех требований с директорами не найдена"));

        /// <summary>
        /// Тип приложения
        /// </summary>
        public override StampApplicationType StampApplicationType => StampApplicationType.Word;

        /// <summary>
        /// Формат
        /// </summary>
        public override StampPaperSizeType PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override StampOrientationType Orientation { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => $"{StampMarkersWord.StampTypeToString[StampType]}";
    }
}
