using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
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
        /// Формат
        /// </summary>
        public override string PaperSize =>
            StampCellElement.SubElements.
            FirstOrDefault(subElement => subElement.IsTextElementMicrostation &&
                                         StampFieldMain.IsFormatField(subElement.AsTextElementMicrostation.Text)).
            Map(subElement => StampFieldMain.GetPaperSizeFromField(subElement.AsTextElementMicrostation.Text));

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override StampOrientationType Orientation => 
            StampCellElement.Range.Width >= StampCellElement.Range.Height 
            ? StampOrientationType.Landscape
            : StampOrientationType.Portrait;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        public override IStampSignatureFields StampSignatureFields =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows, 
                                                       GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary),
                                                       GetStampApprovalRows()));
    }
}
