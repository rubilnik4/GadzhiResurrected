using GadzhiMicrostation.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Модель хранения данных конвертации
    /// </summary>
    public class MicrostationProject : IMicrostationProject
    {
        /// <summary>
        /// Ошибки конвертации
        /// </summary>
        private List<ErrorMicrostation> _errorsMicrostation;

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IProjectMicrostationSettings ProjectMicrostationSettings { get; }

        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        public FileDataMicrostation FileDataMicrostation { get; private set; }

        public MicrostationProject(IProjectMicrostationSettings projectMicrostationSettings)
        {
            _errorsMicrostation = new List<ErrorMicrostation>();

            ProjectMicrostationSettings = projectMicrostationSettings;
        }

        /// <summary>
        /// Ошибки конвертации
        /// </summary>
        public IEnumerable<ErrorMicrostation> ErrorsMicrostation => _errorsMicrostation;

        /// <summary>
        /// Добавить ошибку
        /// </summary>   
        public void AddError(ErrorMicrostation errorMicrostation)
        {
            _errorsMicrostation.Add(errorMicrostation);
        }

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        public void SetInitialFileData(FileDataMicrostation fileDataMicrostation)
        {
            FileDataMicrostation = fileDataMicrostation;
        }
    }
}
