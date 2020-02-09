using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Infrastructure.Interfaces.Converters;
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
        private readonly IFilesDataPackages _filesDataPackages;

        /// <summary>
        /// Конвертер из трансферной модели в серверную
        /// </summary>     
        private readonly IConverterServerFilesDataFromDTO _converterServerFilesDataFromDTO;

        /// <summary>
        /// Конвертер из серверной модели в трансферную
        /// </summary>
        private readonly IConverterServerFilesDataToDTO _converterServerFilesDataToDTO;

        public ApplicationConverting(IFilesDataPackages fileDataPackages,
                                     IConverterServerFilesDataFromDTO converterServerFilesDataFromDTO,
                                     IConverterServerFilesDataToDTO converterServerFilesDataToDTO)
        {
            _filesDataPackages = fileDataPackages;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;

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
            //FilesDataServer filesDataServer = await FilesDataFromDTOConverterToServer.ConvertToFilesDataServerAndSaveFile(filesDataRequest, FileSystemOperations);
            //FilesDataServer filesDataServer1 = await FilesDataFromDTOConverterToServer.ConvertToFilesDataServerAndSaveFile(filesDataRequest, FileSystemOperations);
            //FilesDataServer filesDataServer2 = await FilesDataFromDTOConverterToServer.ConvertToFilesDataServerAndSaveFile(filesDataRequest, FileSystemOperations);

            //filesDataServer.ID = Guid.NewGuid();
            //filesDataServer1.ID = Guid.NewGuid();

            //QueueFilesData(filesDataServer);
            //QueueFilesData(filesDataServer1);
            //QueueFilesData(filesDataServer2);
            FilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
           
            QueueFilesData(filesDataServer);
            
            return await GetIntermediateFilesDataResponseByID(filesDataRequest.ID);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        public void QueueFilesData(FilesDataServer filesDataServer)
        {
            _filesDataPackages.QueueFilesData(filesDataServer);
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        public async Task<FilesDataIntermediateResponse> GetIntermediateFilesDataResponseByID(Guid filesDataServerID)
        {
            FilesDataServer filesDataServer = _filesDataPackages.GetFilesDataServerByID(filesDataServerID);

            FilesDataIntermediateResponse filesDataIntermediateResponse =
                _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer);

            return await Task.FromResult(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        public async Task<FilesDataResponse> GetFilesDataResponseByID(Guid filesDataServerID)
        {
            FilesDataServer filesDataServer = _filesDataPackages.GetFilesDataServerByID(filesDataServerID);
            FilesDataResponse filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
            return filesDataResponse;
        }

        /// <summary>
        /// Конвертировать первый в очереди пакет
        /// </summary>
        public async Task ConvertingFirstInQueuePackage()
        {
            IsConverting = true;

            FilesDataServer filesDataServerFirstInQueue = _filesDataPackages.GetFirstUncompliteInQueuePackage();
            await _filesDataPackages.ConvertingFilesDataPackage(filesDataServerFirstInQueue);

            IsConverting = false;
        }


        public void Dispose()
        {
            //очистить подписки на конвертирование пакетов
            ConvertingUpdaterSubsriptions?.Dispose();
        }
    }
}