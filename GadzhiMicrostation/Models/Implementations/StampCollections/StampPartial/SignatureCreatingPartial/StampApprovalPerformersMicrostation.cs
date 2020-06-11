using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки согласования для списка исполнителей
    /// </summary>
    public partial class SignatureCreatingMicrostation
    {
        /// <summary>
        /// Получить строки согласования для списка исполнителей
        /// </summary>
        public override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>());
    }
}