using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiConverting.Models.Interfaces
{
    /// <summary>
    /// Вариант ответа для конвертации файлов
    /// </summary>
    public interface IResultFileDataSource: IResultConvertingValue<IEnumerable<IFileDataSourceServer>>
    {
        /// <summary>
        /// Добавить ответ
        /// </summary>      
        IResultFileDataSource ConcatResult(IResultFileDataSource resultFileDataSource);
    }
}
