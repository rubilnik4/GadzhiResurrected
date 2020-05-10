using System.Collections.Generic;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;

namespace GadzhiMicrostation.Models.Interfaces.StampFieldNames
{
    /// <summary>
    /// Строка с подписью
    /// </summary>
    public interface IStampFieldSignature
    {
        /// <summary>
        /// Список всех полей
        /// </summary>
        HashSet<StampFieldBase> StampSignatureFields { get; }
    }
}