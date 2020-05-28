using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures);

        /// <summary>
        /// Объединить подписи
        /// </summary>        
        protected static IResultAppCollection<IStampSignature> GetSignatures(IResultAppCollection<IStampPerson> personSignatures,
                                                                             IResultAppCollection<IStampChange> changeSignatures,
                                                                             IResultAppCollection<IStampApproval> approvalSignatures) =>
            personSignatures.Cast<IStampPerson, IStampSignature>().
                             ConcatValues(changeSignatures.Value.Cast<IStampSignature>()).
                             ConcatValues(approvalSignatures.Value.Cast<IStampSignature>());
    }
}