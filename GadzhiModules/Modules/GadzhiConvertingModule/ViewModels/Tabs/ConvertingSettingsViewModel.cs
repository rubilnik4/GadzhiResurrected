using System;
using System.Collections.Generic;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
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

        public ConvertingSettingsViewModel(IProjectSettings projectSettings, IApplicationGadzhi applicationGadzhi)
        {
            if (applicationGadzhi == null) throw new ArgumentNullException(nameof(applicationGadzhi));
            _convertingSettings = projectSettings.ConvertingSettings ?? throw new ArgumentNullException(nameof(projectSettings));

            PersonSignatures = NotifyTaskCompletion.Create(applicationGadzhi.GetSignaturesNames());
            Departments = NotifyTaskCompletion.Create(applicationGadzhi.GetSignaturesDepartments());
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Параметрюшки";

        /// <summary>
        /// Подпись
        /// </summary>
        public ISignatureLibrary PersonSignature
        {
            get => _convertingSettings.PersonSignature;
            set => _convertingSettings.PersonSignature = value;
        }

        /// <summary>
        /// Подписи
        /// </summary>
        public INotifyTaskCompletion<IReadOnlyList<ISignatureLibrary>> PersonSignatures { get; }

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