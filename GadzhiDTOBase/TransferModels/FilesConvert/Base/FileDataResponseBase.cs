using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий ответные данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public abstract class FileDataResponseBase<TFileDataSourceResponse> : FileDataIntermediateResponseBase
        where TFileDataSourceResponse: FileDataSourceResponseBase
    {
        /// <summary>
        /// Информация об отконвертированных файлах в серверной части
        /// </summary>
        [DataMember]
        public abstract IList<TFileDataSourceResponse> FilesDataSource { get; set; }
    }
}
