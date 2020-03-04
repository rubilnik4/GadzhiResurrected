using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiWord.Models.Implementations.FilesConvert;
using GadzhiWord.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Interfaces
{
    /// <summary>
    /// Модель хранения данных конвертации Word
    /// </summary>
    public interface IWordProject
    {
        /// <summary>
        /// Класс для хранения информации о конвертируемом файле типа DGN
        /// </summary>
        IFileDataServerWord FileDataServerWord { get; }

        /// <summary>
        /// Список используемых принтеров
        /// </summary>
        IPrintersInformation PrintersInformation { get; }

        /// <summary>
        /// Записать исходные данные для конвертации
        /// </summary>      
        void SetInitialFileData(IFileDataServer fileDataServer, IPrintersInformation printersInformation);

        /// <summary>
        /// Создать путь для сохранения отконвертированных файлов
        /// </summary>        
        string CreateFileSavePath(string fileName, FileExtention fileExtentionType);
    }
}
