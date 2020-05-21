using System;
using System.Collections.Generic;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ConvertingSettings;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ConvertingSettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IConvertingSettings _convertingSettings;

        public ConvertingSettingsViewModel(IProjectSettings projectSettings)
        {
            _convertingSettings = projectSettings.ConvertingSettings ?? throw new ArgumentNullException(nameof(projectSettings));
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Параметрюшки";

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department
        {
            get => _convertingSettings.Department;
            set => _convertingSettings.Department = value;
        }

        /// <summary>
        /// Отделы
        /// </summary>
        public IReadOnlyList<string> Departments => ConvertingSettings.Departments;
    }
}