﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces 
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public interface IMessageAndLoggingService
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        void ShowError(FileConvertErrorType fileConvertErrorType,
                       string fileConvertErrorDescription);

        /// <summary>
        /// </summary>        
        /// </summary>        
        void ShowMessage(string message);
    }
}