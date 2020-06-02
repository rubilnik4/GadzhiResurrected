using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Поля штампа, отвечающие за подписи
    /// </summary>
    public class StampSignatureFields : IStampSignatureFields
    {
        public StampSignatureFields(IResultAppCollection<IStampChange> stampChanges)
          : this(new ResultAppCollection<IStampPerson>(Enumerable.Empty<IStampPerson>()), stampChanges)
        { }

        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges)
            : this(stampPersons, stampChanges,
                  new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>()))
        { }

        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges,
                                    IResultAppCollection<IStampApproval> stampApproval)
        {
            StampPersons = stampPersons ?? throw new ArgumentNullException(nameof(stampPersons));
            StampChanges = stampChanges ?? throw new ArgumentNullException(nameof(stampChanges));
            StampApproval = stampApproval ?? throw new ArgumentNullException(nameof(stampApproval));
        }

        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        public IResultAppCollection<IStampPerson> StampPersons { get; }

        /// <summary>
        /// Строки с изменениями
        /// </summary>
        public IResultAppCollection<IStampChange> StampChanges { get; }

        /// <summary>
        /// Строки с согласованием
        /// </summary>
        public IResultAppCollection<IStampApproval> StampApproval { get; }

        /// <summary>
        /// Получить все подписи
        /// </summary>        
        public IResultAppCollection<IStampSignature> GetSignatures() =>
            StampPersons.Cast<IStampPerson, IStampSignature>().
            ConcatValues(StampChanges.Value.Cast<IStampSignature>()).
            ConcatValues(StampApproval.Value.Cast<IStampSignature>());
    }
}