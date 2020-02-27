using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий ответные данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public abstract class FileDataResponseBase : FileDataIntermediateResponseBase
    {
       
    }
}
