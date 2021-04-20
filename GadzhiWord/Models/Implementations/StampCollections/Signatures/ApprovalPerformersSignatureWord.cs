using System;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка согласования для опросных листов и тех требований
    /// </summary>
    public class ApprovalPerformersSignatureWord : SignatureWord, IStampApprovalPerformers
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 4;

        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int MAX_ROWS_COUNT = 8;

        public ApprovalPerformersSignatureWord(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier, IStampFieldWord signature,
                                               IStampTextField responsiblePerson, IStampTextField department, IStampTextField dateSignature)
            : base(signatureLibrary, stampIdentifier, signature)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            Department = department ?? throw new ArgumentNullException(nameof(department));
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public IStampTextField Department { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampTextField DateSignature { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampTextField ResponsiblePerson { get; }
    }
}