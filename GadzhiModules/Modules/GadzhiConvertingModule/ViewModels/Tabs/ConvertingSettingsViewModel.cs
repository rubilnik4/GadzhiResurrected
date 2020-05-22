using System;
using System.Collections.Generic;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;
using Nito.AsyncEx;

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

        /// <summary>
        /// Слой приложения, инфраструктура
        /// </summary>
        //private readonly IApplicationGadzhi _applicationGadzhi;

        public ConvertingSettingsViewModel(IProjectSettings projectSettings, IApplicationGadzhi applicationGadzhi)
        {
            if (applicationGadzhi == null) throw new ArgumentNullException(nameof(applicationGadzhi));
            _convertingSettings = projectSettings.ConvertingSettings ?? throw new ArgumentNullException(nameof(projectSettings));

            Departments = NotifyTaskCompletion.Create(applicationGadzhi.GetSignaturesDepartments());
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
        public INotifyTaskCompletion<IList<string>> Departments { get; }
    }
}