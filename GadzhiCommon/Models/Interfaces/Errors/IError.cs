using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        ErrorConvertingType ErrorConvertingType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        string Description { get; }
    }
}