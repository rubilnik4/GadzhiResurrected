using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiDTOServer.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов.Контракт используется серверной частью
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IFileConvertingServerService: IDisposable
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>    
        [OperationContract(IsInitiating = true,
                           IsTerminating = false)]
        Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        [OperationContract(IsInitiating = false,
                           IsTerminating = false)]
        Task UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>  
        [OperationContract(IsInitiating = false,
                           IsTerminating = false)]
        Task UpdateFromResponse(FilesDataResponseServer filesDataResponse);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract(IsInitiating = false,
                          IsTerminating = true)]
        Task AbortConvertingById(Guid id);
    }
}

