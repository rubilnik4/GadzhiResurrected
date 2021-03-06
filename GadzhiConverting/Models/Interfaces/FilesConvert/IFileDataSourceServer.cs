﻿using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Implementations.FilesConvert;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Обработанный файл серверной части
    /// </summary>
    public interface  IFileDataSourceServer: IFilePath, IEnumerable<IFileDataSourceServer>
    {
        /// <summary>
        /// Тип конвертации
        /// </summary>
        ConvertingModeType ConvertingModeType { get; }

        /// <summary>
        /// Формат печати
        /// </summary>
        StampPaperSizeType PaperSize { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        string PrinterName { get; }
    }
}
