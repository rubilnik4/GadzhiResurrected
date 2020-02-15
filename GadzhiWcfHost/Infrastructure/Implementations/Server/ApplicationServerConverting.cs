using GadzhiDAL.Services.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations.Server
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationServerConverting : IApplicationServerConverting
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>
        private readonly IFilesDataServerService _filesDataServerService;       

        public ApplicationServerConverting(IFilesDataServerService filesDataServerService)
        {
            _filesDataServerService = filesDataServerService;          
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>           
        public async Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            return await _filesDataServerService.GetFirstInQueuePackage(identityServerName);
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse)
        {
            await _filesDataServerService.UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task UpdateFromResponse(FilesDataResponseServer filesDataResponse)
        {
            await _filesDataServerService.UpdateFromResponse(filesDataResponse);
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id)
        {
            await _filesDataServerService.AbortConvertingById(id);
        }
    }
}