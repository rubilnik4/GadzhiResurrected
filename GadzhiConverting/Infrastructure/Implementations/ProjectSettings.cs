using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.Printers;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приолжения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {      
        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public string ConvertingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                             "//Converting.gitignore";       

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        public IPrintersInformation PrintersInformation => ConverterPrintingConfiguration.ToPrintersInformation();

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        public int IntervalSecondsToServer => 5;

        /// <summary>
        /// Время через которое осуществляется удаление ненужных пакетов на сервере
        /// </summary>
        public int IntervalHouresToDeleteUnusedPackages => 12;

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public string NetworkName => Environment.UserDomainName + "\\" + Environment.MachineName;

    }
}
