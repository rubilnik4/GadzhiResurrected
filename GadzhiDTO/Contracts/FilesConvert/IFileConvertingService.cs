using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется и клиентской и серверной частью
    /// </summary>
    [ServiceContract]
    public interface IFileConvertingService
    {
        /// <summary>
        /// Отправить файлы для конвертирования
        /// </summary>
        [OperationContract]
        Task<bool> SendFiles(FilesDataRequest filesDataRequest);

        [OperationContract]
        Task Send(FilesDataRequest filesDataRequest);
    }
}
