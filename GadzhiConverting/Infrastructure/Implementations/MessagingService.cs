using GadzhiCommon.Converters;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public class MessagingService : IMessagingService
    {
        /// <summary>
        /// Запись в журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService;

        public MessagingService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>        
        public virtual void ShowAndLogMessage(string message)
        {
            _loggerService.LogMessage(message);
            ShowMessage(message);
        }

        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>            
        public virtual void ShowAndLogError(IErrorCommon errorConverting)
        {
            ShowError(errorConverting);
            _loggerService.LogError(errorConverting);
        }

        /// <summary>
        /// Отобразить и добавить в журнал ошибки
        /// </summary>       
        public virtual void ShowAndLogErrors(IEnumerable<IErrorCommon> errorsConverting)
        {
            foreach (var error in errorsConverting.EmptyIfNull())
            {
                ShowError(error);
                _loggerService.LogError(error);
            }
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        protected virtual void ShowMessage(string message) => Console.WriteLine(message);

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        protected virtual void ShowError(IErrorCommon errorConverting) =>
            errorConverting?.
            Map(error => new List<string>()
            { 
                "Ошибка | " + ConverterErrorType.FileErrorTypeToString(error.FileConvertErrorType),
                errorConverting.ErrorDescription,
                errorConverting.ExceptionMessage}).
            Map(messages => String.Join("\n", messages.Where(message => !String.IsNullOrWhiteSpace(message)))).
            Map(messageText => { Console.WriteLine(messageText); return Unit.Value; });
    }
}
