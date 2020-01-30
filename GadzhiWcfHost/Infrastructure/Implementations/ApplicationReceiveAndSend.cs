using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers.Converters;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
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
    public class ApplicationReceiveAndSend : IApplicationReceiveAndSend
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }

        /// <summary>
        /// Класс для функций конвертирования файлов
        /// </summary>
        private IApplicationConverting ApplicationConverting { get; }

        public ApplicationReceiveAndSend(IFileSystemOperations fileSystemOperations,
                                        IApplicationConverting applicationConverting)
        {
            FileSystemOperations = fileSystemOperations;
            ApplicationConverting = applicationConverting;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            var filesDataServerWithErrorsTask = filesDataRequest?.FilesData?.Select(fileDTO =>
                                    FilesDataFromDTOConverterToServer.ConvertToFileDataServerAndSaveFile(fileDTO, FileSystemOperations));
            var filesDataServerWithErrors = await Task.WhenAll(filesDataServerWithErrorsTask);

            //файлы с ошибками просто не будут обработаны
            var filesDataServerToQueue = new FilesDataServer(filesDataServerWithErrors);
            ApplicationConverting.QueueFilesData(filesDataServerToQueue);

            return new FilesDataIntermediateResponse()
            {
                FilesData = filesDataServerWithErrors?.Select(fileDataServer =>
                            FileDataServerToDTOConverter.ConvertToIntermediateResponse(fileDataServer)),
            };
        }
    }
}