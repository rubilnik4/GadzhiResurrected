using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки согласования тех требований с директорами Microstation
    /// </summary>
    public partial class SignatureCreatingMicrostation
    {
        /// <summary>
        /// Получить строки согласования тех требований с директорами Microstation
        /// </summary>
        public override IResultAppCollection<IStampApprovalChief> GetStampApprovalChiefRows() =>
            new ResultAppCollection<IStampApprovalChief>(Enumerable.Empty<IStampApprovalChief>());
    }
}