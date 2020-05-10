using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public interface IStampApprovalMicrostation : IStampApproval<IStampFieldMicrostation>, IStampSignatureMicrostation
    {
        /// <summary>
        /// Тип действия
        /// </summary>
        ITextElementMicrostation DepartmentApprovalElement { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        ITextElementMicrostation ResponsiblePersonElement { get; }

        /// <summary>
        /// Дата
        /// </summary>
        IResultAppValue<ICellElementMicrostation> SignatureElement { get; }

        /// <summary>
        /// Дата
        /// </summary>
        ITextElementMicrostation DateSignatureElement { get; }       
    }
}
