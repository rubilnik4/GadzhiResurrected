using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Основная модель состояния процесса конвертирования
    /// </summary>
    public class ConvertingProject
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        private List<ErrorTypeConverting> _errorsTypeConverting;

        public ConvertingProject()
        {
            _errorsTypeConverting = new List<ErrorTypeConverting>();
        }

        public void AddError()
        {

        }
    }
}
