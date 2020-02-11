using GadzhiConverting.Models.Implementations.ReactiveSubjects;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Основная модель состояния процесса конвертирования
    /// </summary>
    public class ConvertingProject: IConvertingProject
    {  
        public ConvertingProject()
        {
            _errorsTypeConverting = new List<ErrorTypeConverting>();
        }

        /// <summary>
        /// Список ошибок
        /// </summary>
        private List<ErrorTypeConverting> _errorsTypeConverting;

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<ConvertingProjectChange> ConvertingProjectChange { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        public void AddError(ErrorTypeConverting errorTypeConverting)
        {
            if (errorTypeConverting != null)
            {
                _errorsTypeConverting?.Add(errorTypeConverting);
            }
            else
            {
                throw new ArgumentNullException("Ошибка не инициализирована");
            }
        }

        /// <summary>
        /// Зарегистрировать изменение
        /// </summary>
        private void UpdateconvertingProject(ConvertingProjectChange convertingProjectChange)
        {
            ConvertingProjectChange?.OnNext(convertingProjectChange);
        }
    }
}
