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
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        public void ShowError(FileConvertErrorType fileConvertErrorType,
                              string fileConvertErrorDescription)
        {
            string messageText = "Ошибка | " +
                                 ConverterErrorType.FileErrorTypeToString(fileConvertErrorType) + "\n" +
                                 fileConvertErrorDescription;

            Console.WriteLine(messageText);
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>        
        public void ShowMessage(string message)
        {
            string messageText = message;

            Console.WriteLine(messageText);
        }
    }
}
