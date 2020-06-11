using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampTypes;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Основные поля штампа Microstation
    /// </summary>
    public class StampFullMicrostation : StampMicrostation, IStampFull
    {
        public StampFullMicrostation(ICellElementMicrostation stampCellElement, StampSettings stampSettings,
                                     SignaturesSearching signaturesSearching)
            : base(stampSettings, stampCellElement, signaturesSearching)
        { }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Full;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            SignatureCreating.GetStampPersonRows().
            Map(personRows => new StampSignatureFields(new SignaturesBuilder().
                                                       AddStampPersons(personRows).
                                                       AddStampChanges(SignatureCreating.GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary)).
                                                       AddStampApprovals(SignatureCreating.GetStampApprovalRows())));
    }
}
