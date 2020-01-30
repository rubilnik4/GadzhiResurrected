using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers.Converters;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
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
    public class ApplicationConverting : IApplicationConverting
    {
        /// <summary>
        /// Класс пользовательских пакетов на конвертирование
        /// </summary>
        public IFilesDataPackages FileDataPackages { get; }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }


        public ApplicationConverting(IFilesDataPackages fileDataPackages,
                                     IFileSystemOperations fileSystemOperations)
        {
            FileDataPackages = fileDataPackages;
            FileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            FilesDataServer filesDataServer = await FilesDataFromDTOConverterToServer.ConvertToFilesDataServerAndSaveFile(filesDataRequest, FileSystemOperations);
            QueueFilesData(filesDataServer);

            return GetIntermediateResponseByID(filesDataServer.ID);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            FileDataPackages.QueueFilesData(filesDataServer);
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        public FilesDataIntermediateResponse GetIntermediateResponseByID(Guid filesDataServerID)
        {
            FilesDataServer filesDataServer = FileDataPackages.GetFilesDataServerByID(filesDataServerID);

            FilesDataIntermediateResponse filesDataIntermediateResponse = 
                FilesDataServerToDTOConverter.ConvertFilesToIntermediateResponse(filesDataServer);

            return filesDataIntermediateResponse;
        }
    }
}