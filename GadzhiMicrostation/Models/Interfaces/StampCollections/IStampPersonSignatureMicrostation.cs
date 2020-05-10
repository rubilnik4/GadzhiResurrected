using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public interface IStampPersonMicrostation : IStampPerson<IStampFieldMicrostation>, IStampSignatureMicrostation
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
        IResultAppValue<ICellElementMicrostation> SignatureElement { get; }

        /// <summary>
        /// Дата
        /// </summary>
        ITextElementMicrostation DateSignatureElement { get; }
    }
}
