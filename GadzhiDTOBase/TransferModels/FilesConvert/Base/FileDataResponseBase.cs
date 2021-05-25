using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий ответные данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public abstract class FileDataResponseBase<TFileDataSourceResponse> : FileDataShortResponseBase
        where TFileDataSourceResponse : FileDataSourceResponseBase
    {
        protected FileDataResponseBase(string filePath, StatusProcessing statusProcessing,
                                       IList<ErrorCommonResponse> fileErrors, IList<TFileDataSourceResponse> filesDataSource)
            : base(filePath, statusProcessing, fileErrors)
        {
            FilesDataSource = filesDataSource;
        }

        /// <summary>
        /// Информация об отконвертированных файлах в серверной части
        /// </summary>
        [DataMember]
        public IList<TFileDataSourceResponse> FilesDataSource { get; private set; }
    }
}
