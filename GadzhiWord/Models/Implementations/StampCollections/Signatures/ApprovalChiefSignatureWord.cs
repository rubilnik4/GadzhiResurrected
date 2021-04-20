using System;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строки согласования для опросных листов и тех требований с директорами
    /// </summary>
    public class ApprovalChiefSignatureWord : SignatureWord, IStampApprovalChief
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 3;

        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int MAX_ROWS_COUNT = 6;

        public ApprovalChiefSignatureWord(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier, IStampFieldWord signature,
                                          IStampTextField responsiblePerson, IStampTextField department)
            : base(signatureLibrary,stampIdentifier, signature)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            Department = department ?? throw new ArgumentNullException(nameof(department));
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public IStampTextField Department { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampTextField ResponsiblePerson { get; }
    }
}