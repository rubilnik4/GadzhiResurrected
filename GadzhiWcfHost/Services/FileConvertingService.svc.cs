using GadzhiDTO.Contracts.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GadzhiWcfHost.Services
{
    //Обязательно сверять путь с файлом App.config
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется и клиентской и серверной частью
    /// </summary>
    public class FileConvertingService : IFileConvertingService
    {       
        public int SendFiles()
        {
            Console.WriteLine("Поихали");

            return DateTime.Now.Second;
        }
    }
}
