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
        private readonly IProjectSettings _projectSettings;

        public ConvertingSettingsViewModel(IProjectSettings projectSettings)
        {
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            Department = projectSettings.ConvertingSettings.Department;
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Параметрюшки";

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Отделы
        /// </summary>
        public IReadOnlyList<string> Departments => ConvertingSettings.Departments;

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        public void SaveSettingsChanges() => _projectSettings.UpdateConvertingSettings(Department);
    }
}