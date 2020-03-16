using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public interface IStampPersonSignatureMicrostation : IStampPersonSignature<IStampFieldMicrostation>
    {
        /// <summary>
        /// Тип действия
        /// </summary>
        ITextElementMicrostation ActionTypeElement { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        ITextElementMicrostation ResponsiblePersonElement { get; }

        /// <summary>
        /// Дата
        /// </summary>
        ICellElementMicrostation SignatureElement { get; }

        /// <summary>
        /// Дата
        /// </summary>
        ITextElementMicrostation DateSignatureElement { get; }
    }
}
