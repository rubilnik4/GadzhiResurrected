using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataResponseBase<TFileDataResponse, TFileDataSourceResponse>:
        PackageDataShortResponseBase<TFileDataResponse>
        where TFileDataSourceResponse : FileDataSourceResponseBase
        where TFileDataResponse : FileDataResponseBase<TFileDataSourceResponse>
    {
        protected PackageDataResponseBase(Guid id, StatusProcessingProject statusProcessingProject,
                                          IList<TFileDataResponse> filesData)
            :base(id, statusProcessingProject, filesData)
        { }
    }
}
