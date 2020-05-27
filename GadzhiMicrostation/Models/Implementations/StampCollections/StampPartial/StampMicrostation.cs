using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public abstract partial class StampMicrostation : Stamp, IStampMicrostation
    {
        protected StampMicrostation(StampSettings stampSettings, ICellElementMicrostation stampCellElement, 
                                    SignaturesLibrarySearching signaturesLibrarySearching)
            : base(stampSettings)
        {
            StampCellElement = stampCellElement ?? throw new ArgumentNullException(nameof(stampCellElement));
            SignaturesLibrarySearching = signaturesLibrarySearching ?? throw new ArgumentNullException(nameof(signaturesLibrarySearching));

            StampSubControls = FindElementsInStampFields(stampCellElement.SubElements, StampFieldElement.StampControlNames).ToList();
        }

        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        public ICellElementMicrostation StampCellElement { get; }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        protected SignaturesLibrarySearching SignaturesLibrarySearching { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => StampCellElement.Name;

        /// <summary>
        /// Формат
        /// </summary>
        public override string PaperSize => StampCellElement.SubElements?.
                                            FirstOrDefault(subElement => subElement.IsTextElementMicrostation &&
                                                                         StampFieldMain.IsFormatField(subElement.AsTextElementMicrostation.Text))?.
                                            Map(subElement => StampFieldMain.GetPaperSizeFromField(subElement.AsTextElementMicrostation.Text)) 
                                            ?? String.Empty;

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override StampOrientationType Orientation => 
            StampCellElement.Range.Width >= StampCellElement.Range.Height 
            ? StampOrientationType.Landscape
            : StampOrientationType.Portrait;
    }
}
