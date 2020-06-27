using System;
using System.Collections.Generic;
using System.Reflection;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using Nito.Mvvm;

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
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        public ConvertingSettingsViewModel(IProjectSettings projectSettings, IProjectResources projectResources)
        {
            _convertingSettings = projectSettings.ConvertingSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            PersonSignatures = projectResources.PersonSignatures;
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Параметры";

        /// <summary>
        /// Текст подписи во время загрузки
        /// </summary>
        public string PersonSignatureLoading => (_convertingSettings.PersonSignature?.PersonInformation.HasFullInformation == true)
                                                ? _convertingSettings.PersonSignature?.PersonInformation.FullInformation
                                                : "Выберите подпись";

        /// <summary>
        /// Подпись
        /// </summary>
        public ISignatureLibrary PersonSignature
        {
            get => _convertingSettings.PersonSignature;
            set => _convertingSettings.PersonSignature = value.
                   Void(_ => _loggerService.LogProperty(nameof(PersonSignature), nameof(ConvertingSettingsViewModel), LoggerInfoLevel.Info, value.PersonInformation.FullName));
        }

        /// <summary>
        /// Подписи
        /// </summary>
        public NotifyTask<IReadOnlyList<ISignatureLibrary>> PersonSignatures { get; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public PdfNamingType PdfNamingType
        {
            get => _convertingSettings.PdfNamingType;
            set => _convertingSettings.PdfNamingType = value.
                   Void(_ => _loggerService.LogProperty(nameof(PdfNamingType), nameof(ConvertingSettingsViewModel), LoggerInfoLevel.Info, value.ToString()));
        }

        /// <summary>
        /// Принципы именования PDF
        /// </summary>
        public IReadOnlyDictionary<PdfNamingType, string> PdfNamingTypes => PdfNamingConverter.PdfNamingString;
    }
}