using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Infrastructure.Implementations.Converters;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
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
        /// Отобразить сообщение
        /// </summary>        
        public virtual void ShowAndLogMessage(string message)
        {
            ShowMessage(message);
        }

        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>            
        public virtual void ShowAndLogError(IErrorCommon errorConverting)
        {
            ShowError(errorConverting);
        }

        /// <summary>
        /// Отобразить и добавить в журнал ошибки
        /// </summary>       
        public virtual void ShowAndLogErrors(IEnumerable<IErrorCommon> errorsConverting)
        {
            foreach (var error in errorsConverting.EmptyIfNull())
            {
                ShowError(error);
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
                "Ошибка | " + ConverterErrorType.ErrorTypeToString(error.FileConvertErrorType),
                errorConverting.ErrorDescription,
                errorConverting.ExceptionMessage}).
            Map(messages => String.Join("\n", messages.Where(message => !String.IsNullOrWhiteSpace(message)))).
            Map(messageText => { Console.WriteLine(messageText); return Unit.Value; });
    }
}
