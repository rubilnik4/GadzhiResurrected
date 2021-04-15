using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial
{
    /// <summary>
    /// Класс для работы с приложением конвертации
    /// </summary>
    public interface IApplicationConverting: IApplicationConvertingDocument
    {
        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        FileExtension GetExportFileExtension(FileExtension fileExtensionMain);

        /// <summary>
        /// Закрыть приложения
        /// </summary>
        void CloseApplications();
    }
}
