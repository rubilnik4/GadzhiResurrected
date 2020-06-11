using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки согласования для извещения с изменениями Microstation
    /// </summary>
    public partial class SignatureCreatingMicrostation
    {
        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи Microstation
        /// </summary>
        public override IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows() =>
            new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>());
    }
}