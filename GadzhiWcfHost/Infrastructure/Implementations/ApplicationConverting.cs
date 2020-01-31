using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers.Converters;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationConverting : IApplicationConverting, IDisposable
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

            ConvertingUpdaterSubsriptions = new CompositeDisposable();
            ConvertingUpdaterSubsriptions.Add(Observable.
                                              Interval(TimeSpan.FromSeconds(10)).
                                              TakeWhile(_ => !IsConverting).
                                              Subscribe(async _ => await ConvertingFirstInQueuePackage()));
        }

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private CompositeDisposable ConvertingUpdaterSubsriptions { get; }

        /// <summary>
        /// Запущен ли процесс конвертации
        /// </summary>
        public bool IsConverting { get; private set; }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            FilesDataServer filesDataServer = await FilesDataFromDTOConverterToServer.ConvertToFilesDataServerAndSaveFile(filesDataRequest, FileSystemOperations);
            QueueFilesData(filesDataServer);

            return await GetIntermediateResponseByID(filesDataServer.ID);
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
        public async Task<FilesDataIntermediateResponse> GetIntermediateResponseByID(Guid filesDataServerID)
        {
            FilesDataServer filesDataServer = FileDataPackages.GetFilesDataServerByID(filesDataServerID);

            FilesDataIntermediateResponse filesDataIntermediateResponse =
                FilesDataServerToDTOConverter.ConvertFilesToIntermediateResponse(filesDataServer);

            return await Task.FromResult(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Конвертировать первый в очереди пакет
        /// </summary>
        public async Task ConvertingFirstInQueuePackage()
        {
            IsConverting = true;

            FilesDataServer filesDataServerFirstInQueue = FileDataPackages.GetFirstInQueuePackage();
            await FileDataPackages.ConvertingFilesDataPackage(filesDataServerFirstInQueue);

            IsConverting = false;
        }


        public void Dispose()
        {
            //очистить подписки на конвертирование пакетов
            ConvertingUpdaterSubsriptions?.Dispose();
        }
    }
}