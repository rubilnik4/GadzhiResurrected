using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки с согласованием
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить строки с согласованием без подписи Word
        /// </summary>
        protected override IResultAppCollection<IStampApproval> GetStampApprovalRows() =>
            new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>());
    }
}