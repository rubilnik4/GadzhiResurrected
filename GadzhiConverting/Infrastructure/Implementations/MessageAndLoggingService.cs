using GadzhiCommon.Converters;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.ReactiveSubjects;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public class MessageAndLoggingService : IMessageAndLoggingService
    {
      
        public MessageAndLoggingService()
        {
           
        }
       
        private readonly string _separator = "------------------------------------------" + "\n";

        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        public void ShowError(FileConvertErrorType fileConvertErrorType,
                              string fileConvertErrorDescription)
        {
            string messageText = _separator +
                                 "Ошибка | " +
                                 ConverterErrorTypeToString.ConvertFileConvertErrorTypeToString(fileConvertErrorType) + "\n" +
                                 fileConvertErrorDescription;

            Console.WriteLine(messageText);
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>        
        public void ShowMessage(string message)
        {
            string messageText = _separator +
                                 message;

            Console.WriteLine(messageText);
        }
    }
}
