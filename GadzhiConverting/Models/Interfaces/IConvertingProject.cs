using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Interfaces
{
    /// <summary>
    /// Основная модель состояния процесса конвертирования
    /// </summary>
    public interface IConvertingProject
    {

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        ISubject<ConvertingProjectChange> ConvertingProjectChange { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        void AddError(ErrorTypeConverting errorTypeConverting);
    }
}
