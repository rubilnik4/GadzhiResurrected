using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers.Converters;
using GadzhiWcfHost.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationReceiveAndSend: IApplicationReceiveAndSend
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }

        public ApplicationReceiveAndSend(IFileSystemOperations fileSystemOperations)
        {
            FileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            var filesDataServerWithErrorsTask = filesDataRequest?.FilesData?.Select(fileDTO =>
                                    FilesDataFromDTOConverterToServer.ConvertToFileDataServerAndSaveFile(fileDTO, FileSystemOperations));
            var filesDataServerWithError = await Task.WhenAll(filesDataServerWithErrorsTask);

            return new FilesDataIntermediateResponse()
            {
                FilesData = filesDataServerWithError?.Select(fileDataServer =>
                            FileDataServerToDTOConverter.ConvertToIntermediateResponse(fileDataServer)),
            };
        }
    }
}