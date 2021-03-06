﻿using System;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOServer.TransferModels.FilesConvert;

namespace GadzhiDAL.Services.Interfaces.FileConvert
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public interface IFilesDataServerService
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование в серверной части
        /// </summary>      
        Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        Task<StatusProcessingProject> UpdateFromIntermediateResponse(Guid packageId, FileDataResponseServer fileDataResponseServer);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>      
        Task<Unit> UpdateFromResponse(PackageDataShortResponseServer packageDataShortResponseServer);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task<Unit> AbortConvertingById(Guid id);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion);
    }
}
