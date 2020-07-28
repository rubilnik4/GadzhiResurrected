using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public class MessagingService : IMessagingService
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        public void ShowMessage(string message) => Console.WriteLine(message);

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        public void ShowError(IErrorCommon errorConverting) =>
            errorConverting?.
            Map(error => new List<string>()
            {
                "Ошибка | " + ConverterErrorType.ErrorTypeToString(error.ErrorConvertingType),
                errorConverting.Description,
                errorConverting.Exception?.Message}).
            Map(messages => String.Join("\n", messages.Where(message => !String.IsNullOrWhiteSpace(message)))).
            Map(messageText => { Console.WriteLine(messageText); return Unit.Value; });

        /// <summary>
        /// Отобразить и добавить в журнал ошибки
        /// </summary>       
        public void ShowErrors(IEnumerable<IErrorCommon> errorsConverting)
        {
            foreach (var error in errorsConverting.EmptyIfNull())
            {
                ShowError(error);
            }
        }

        /// <summary>
        /// Отобразить и записать в лог ошибку
        /// </summary>
        public void ShowAndLogError(IErrorCommon errorConverting) =>
            errorConverting.
            Void(ShowError).
            Void(error => _loggerService.ErrorLog(error));

        /// <summary>
        /// Отобразить и записать в лог ошибки
        /// </summary>
        public void ShowAndLogErrors(IEnumerable<IErrorCommon> errorsConverting) =>
            errorsConverting.
            Void(ShowErrors).
            Void(error => _loggerService.ErrorsLog(error));


    }
}
