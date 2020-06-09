using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки согласования для списка исполнителей
    /// </summary>
    public partial class StampMicrostation
    {
        /// <summary>
        /// Получить строки согласования для списка исполнителей
        /// </summary>
        protected override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>());
    }
}