using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;

// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.FilesConvert.Components
{
    /// <summary>
    /// Информация об ошибке
    /// </summary>
    public class ErrorComponent: IError
    {
        public ErrorComponent()
        { }

        public ErrorComponent(ErrorConvertingType errorConvertingType, string description)
        {
            ErrorConvertingType = errorConvertingType;
            Description = description;
        }

        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        public virtual ErrorConvertingType ErrorConvertingType { get; protected set; } =
            ErrorConvertingType.NoError;

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public virtual string Description { get; protected set; } = 
            String.Empty;
    }
}