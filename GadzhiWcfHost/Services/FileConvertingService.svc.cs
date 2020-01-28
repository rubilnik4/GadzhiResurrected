using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Services
{
    //Обязательно сверять путь с файлом App.config
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется и клиентской и серверной частью
    /// </summary>
    public class FileConvertingService : IFileConvertingService
    {
        public async Task<FilesDataIntermediateResponse> SendFiles(FilesDataRequest filesDataRequest)
        {
            await Task.Delay(5000);

            var filesDataIntermediateResponse = new FilesDataIntermediateResponse()
            {
                FilesData = filesDataRequest.FilesData.Select(
                     file => new FileDataIntermediateResponse()
                     {
                         FilePath = file.FilePath,
                         StatusProcessing = StatusProcessing.InQueue,
                         
                     })
            };
            return filesDataIntermediateResponse;
        }
    }
}
