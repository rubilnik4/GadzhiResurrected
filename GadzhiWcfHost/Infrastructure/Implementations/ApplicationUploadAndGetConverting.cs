using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Services.Implementations;
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
    public class ApplicationUploadAndGetConverting : IApplicationUploadAndGetConverting
    {
        ///// <summary>
        ///// Класс пользовательских пакетов на конвертирование
        ///// </summary>
        //private readonly IFilesDataPackages _filesDataPackages;

        ///// <summary>
        ///// Конвертер из трансферной модели в серверную
        ///// </summary>     
        //private readonly IConverterServerFilesDataFromDTO _converterServerFilesDataFromDTO;

        ///// <summary>
        ///// Конвертер из серверной модели в трансферную
        ///// </summary>
        //private readonly IConverterServerFilesDataToDTO _converterServerFilesDataToDTO;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах
        /// </summary>
        private readonly IFilesDataService _filesDataService;

        ///// <summary>
        ///// Запуск процесса конвертирования
        ///// </summary>
        //private readonly CompositeDisposable _convertingUpdaterSubsriptions;

        public ApplicationUploadAndGetConverting(IFilesDataService filesDataService)
        {           
            _filesDataService = filesDataService;

            //_convertingUpdaterSubsriptions = new CompositeDisposable
            //{
            //    Observable.Interval(TimeSpan.FromSeconds(10)).
            //               TakeWhile(_ => !IsConverting).
            //               Subscribe(async _ => await ConvertingFirstInQueuePackage())
            //};
        }

        ///// <summary>
        ///// Запущен ли процесс конвертации
        ///// </summary>
        //public bool IsConverting { get; private set; }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest)
        {
            await QueueFilesData(filesDataRequest);

            return await GetIntermediateFilesDataResponseById(filesDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(FilesDataRequest filesDataRequest)
        {
            await _filesDataService.QueueFilesData(filesDataRequest);
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        public async Task<FilesDataIntermediateResponse> GetIntermediateFilesDataResponseById(Guid filesDataId)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse =
                    await _filesDataService.GetIntermediateFilesDataById(filesDataId);

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        public async Task<FilesDataResponse> GetFilesDataResponseByID(Guid filesDataServerID)
        {
            //FilesDataServer filesDataServer = _filesDataPackages.GetFilesDataServerByID(filesDataServerID);
            //FilesDataResponse filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
            return null;
        }

        ///// <summary>
        ///// Конвертировать первый в очереди пакет
        ///// </summary>
        //public async Task ConvertingFirstInQueuePackage()
        //{
        //    IsConverting = true;

        //    FilesDataServer filesDataServerFirstInQueue = _filesDataPackages.GetFirstUncompliteInQueuePackage();
        //    await _filesDataPackages.ConvertingFilesDataPackage(filesDataServerFirstInQueue);

        //    IsConverting = false;
        //}
    }
}