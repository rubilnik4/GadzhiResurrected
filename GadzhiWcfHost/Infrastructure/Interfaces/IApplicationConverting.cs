﻿using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для функций конвертирования файлов
    /// </summary>
    public interface IApplicationConverting
    {
        ///// <summary>
        ///// Класс пользовательских пакетов на конвертирование
        ///// </summary>
        //IFilesDataPackages FileDataPackage { get; }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        void QueueFilesData(FilesDataServer filesDataServer);
    }
}