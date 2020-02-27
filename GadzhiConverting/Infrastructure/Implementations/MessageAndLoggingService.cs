using GadzhiCommon.Converters;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using System;

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

        private readonly string _separator = "\n" + "------------------------------------------" + "\n";

        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        public void ShowError(FileConvertErrorType fileConvertErrorType,
                              string fileConvertErrorDescription)
        {
            string messageText = _separator +
                                 "Ошибка | " +
                                 ConverterErrorType.FileErrorTypeToString(fileConvertErrorType) + "\n" +
                                 fileConvertErrorDescription;

            Console.WriteLine(messageText);
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>        
        public void ShowMessage(string message)
        {
            string messageText = message + "\n";

            Console.WriteLine(messageText);
        }
    }
}
