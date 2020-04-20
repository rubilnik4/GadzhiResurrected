using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public interface IStampApprovalSignatureMicrostation : IStampApprovalSignatures<IStampFieldMicrostation>
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
