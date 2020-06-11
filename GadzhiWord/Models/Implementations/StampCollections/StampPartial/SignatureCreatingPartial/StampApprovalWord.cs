using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки с согласованием 
    /// </summary>
    public partial class SignatureCreatingWord
    {
        /// <summary>
        /// Получить строки с согласованием без подписи Word для извещения с изменениями
        /// </summary>
        public override IResultAppCollection<IStampApproval> GetStampApprovalRows() =>
            new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>());
    }
}