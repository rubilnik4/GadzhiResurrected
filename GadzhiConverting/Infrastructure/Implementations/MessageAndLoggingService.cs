using GadzhiCommon.Converters;
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
        /// <summary>
        /// Основная модель состояния процесса конвертирования
        /// </summary>
        //private readonly IConvertingProject _convertingProject;

        public MessageAndLoggingService()
        {
            // _convertingProject = convertingProject;

            // _convertingProject.ConvertingProjectChange.Subscribe(OnConvertingProjectChanged);
        }

        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnConvertingProjectChanged(ConvertingProjectChange convertingProjectChange)
        {
            //if (fileChange.ActionType != ActionType.StatusChange)
            //{

            //    if (fileChange.ActionType == ActionType.Add)
            //    {
            //        ActionOnTypeAdd(fileChange);
            //    }
            //    else if (fileChange.ActionType == ActionType.Remove || fileChange.ActionType == ActionType.Clear)
            //    {
            //        ActionOnTypeRemove(fileChange);
            //    }
            //}
            //ActionOnTypeStatusChange(fileChange);
        }
        private readonly string _separator = "------------------------------------------" + "\n";
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        public void ShowError(ErrorTypeConverting errorTypeConverting)
        {
            string messageText = _separator +
                             "Ошибка | " +
                             ConverterErrorTypeToString.ConvertFileConvertErrorTypeToString(errorTypeConverting.FileConvertErrorType) + "\n" +
                             errorTypeConverting.FileConvertErrorDescription;

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
