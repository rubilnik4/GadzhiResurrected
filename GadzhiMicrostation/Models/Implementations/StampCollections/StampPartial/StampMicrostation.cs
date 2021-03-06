﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Infrastructure.Implementations.Converters.StampCollections;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public abstract partial class StampMicrostation : Stamp, IStampMicrostation
    {
        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        public ICellElementMicrostation StampCellElement { get; }

        protected StampMicrostation(StampSettings stampSettings, ICellElementMicrostation stampCellElement,
                                    SignaturesSearching signaturesSearching)
            : base(stampSettings, signaturesSearching)
        {
            StampCellElement = stampCellElement ?? throw new ArgumentNullException(nameof(stampCellElement));
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => StampCellElement.Name;

        /// <summary>
        /// Тип приложения
        /// </summary>
        public override StampApplicationType StampApplicationType => StampApplicationType.Microstation;

        /// <summary>
        /// Формат
        /// </summary>
        public override StampPaperSizeType PaperSize =>
            StampCellElement.SubElements.
            FirstOrDefault(subElement => subElement.IsTextElementMicrostation &&
                                         StampFieldMain.IsFormatField(subElement.AsTextElementMicrostation.Text)).
            Map(subElement => StampFieldMain.GetPaperSizeFromField(subElement.AsTextElementMicrostation.Text)).
            Map(ConverterStampPaperSize.ToPaperSize);

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override StampOrientationType Orientation =>
            StampCellElement.Range.Width >= StampCellElement.Range.Height
                ? StampOrientationType.Landscape
                : StampOrientationType.Portrait;
    }
}
