using GadzhiMicrostation.Models.Implementations;
using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Interfaces
{
    /// <summary>
    /// Модель хранения данных конвертации
    /// </summary>
    public interface IMicrostationProject
    {
        /// <summary>
        /// Ошибки конвертации
        /// </summary>
        IEnumerable<ErrorMicrostation> ErrorsMicrostation { get; }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        IProjectMicrostationSettings ProjectMicrostationSettings { get; }

        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        FileDataMicrostation FileDataMicrostation { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>   
        void AddError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        void SetInitialFileData(FileDataMicrostation fileDataMicrostation);

        /// <summary>
        /// Создать путь для сохранения отконвертированного файла
        /// </summary>        
        string CreateDngSavePath();
    }
}
