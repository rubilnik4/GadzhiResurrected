using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки согласования со списком исполнителей Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить строки с согласованием без подписи Word для извещения с изменениями
        /// </summary>
        protected override IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows() =>
            new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>());
    }
}