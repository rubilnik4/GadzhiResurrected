using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Строки согласования для извещения с изменениями
    /// </summary>
    public partial class StampMicrostation
    {
        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи Microstation
        /// </summary>
        protected override IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows() =>
            new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>());
    }
}