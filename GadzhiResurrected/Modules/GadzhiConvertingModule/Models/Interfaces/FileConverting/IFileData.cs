using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public interface IFileData
    {
        /// <summary>
        /// Расширение файла
        /// </summary>
        FileExtensionType FileExtensionType { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Путь файла
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        ColorPrintType ColorPrintType { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        IReadOnlyCollection<IErrorCommon> FileErrors { get; }

        /// <summary>
        /// Статус ошибок
        /// </summary>
        StatusErrorType StatusErrorType { get; }

        /// <summary>
        /// Изменить цвет печати
        /// </summary>
        void SetColorPrint(ColorPrintType colorPrintType);

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        IFileData ChangeByFileStatus(FileStatus fileStatus);
    }
}