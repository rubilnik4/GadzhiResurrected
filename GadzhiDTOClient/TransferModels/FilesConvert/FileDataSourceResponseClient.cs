﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в клиентской части
    /// </summary>
    [DataContract]
    public class FileDataSourceResponseClient : FileDataSourceResponseBase
    {
        public FileDataSourceResponseClient(string fileName, FileExtensionType fileExtensionType, string paperSize,
                                            string printerName, byte[] fileDataSource)
            : base(fileName, fileExtensionType, paperSize, printerName, fileDataSource)
        { }
    }
}
