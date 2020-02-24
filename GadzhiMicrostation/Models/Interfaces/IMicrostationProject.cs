using GadzhiMicrostation.Models.Enums;
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
        /// Записать исходные данные для конвертации
        /// </summary>      
        void SetInitialFileData(FileDataMicrostation fileDataMicrostation, PrintersInformation printersInformation);

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
        /// Список используемых принтеров
        /// </summary>
        PrintersInformation PrintersInformation { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>   
        void AddError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Создать путь для сохранения отконвертированных файлов
        /// </summary>        
        string CreateFileSavePath(string fileName, FileExtentionType fileExtentionType);
    }
}
