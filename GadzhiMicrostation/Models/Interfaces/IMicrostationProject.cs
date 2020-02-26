using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
using GadzhiMicrostation.Models.Implementations.Printers;
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
        void SetInitialFileData(FileDataMicrostation fileDataMicrostation, PrintersInformationMicrostation printersInformation);
       
        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        FileDataMicrostation FileDataMicrostation { get; }

        /// <summary>
        /// Список используемых принтеров
        /// </summary>
        PrintersInformationMicrostation PrintersInformation { get; }

        /// <summary>
        /// Создать путь для сохранения отконвертированных файлов
        /// </summary>        
        string CreateFileSavePath(string fileName, FileExtentionMicrostation fileExtentionType);
    }
}
