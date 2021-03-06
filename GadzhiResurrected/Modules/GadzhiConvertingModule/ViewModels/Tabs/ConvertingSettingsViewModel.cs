﻿using System;
using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiResurrected.Infrastructure.Implementations.Converters;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Base;
using Nito.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ConvertingSettingsViewModel : ViewModelBase
    {
        public ConvertingSettingsViewModel(IProjectSettings projectSettings, IProjectResources projectResources,
                                           IDialogService dialogService)
        {
            _convertingSettings = projectSettings?.ConvertingSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            PersonSignatures = projectResources?.PersonSignatures ?? throw new ArgumentNullException(nameof(projectResources));
            DialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IConvertingSettings _convertingSettings;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

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
        [Logger]
        public ISignatureLibrary PersonSignature
        {
            get => _convertingSettings.PersonSignature;
            set => _convertingSettings.PersonSignature = value;
        }

        /// <summary>
        /// Подписи
        /// </summary>
        public NotifyTask<IResultCollection<ISignatureLibrary>> PersonSignatures { get; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        [Logger]
        public PdfNamingType PdfNamingType
        {
            get => _convertingSettings.PdfNamingType;
            set => _convertingSettings.PdfNamingType = value;
        }

        /// <summary>
        /// Принципы именования PDF  
        /// </summary>
        public static IReadOnlyDictionary<PdfNamingType, string> PdfNamingTypes => 
            PdfNamingConverter.PdfNamingString;

        /// <summary>
        /// Цвет печати строковое значение
        /// </summary>
        [Logger]
        public string ColorPrintName
        {
            get => ColorPrintConverter.ColorPrintToString(_convertingSettings.ColorPrintType);
            set => _convertingSettings.ColorPrintType = ColorPrintConverter.ColorPrintFromString(value);
        }

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public IReadOnlyCollection<string> ColorPrintsString =>
            ColorPrintConverter.ColorPrintsString;

        /// <summary>
        /// Типы конвертаций
        /// </summary>
        public IReadOnlyCollection<ConvertingMode> ConvertingModes =>
            _convertingSettings.ConvertingModes;

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        [Logger]
        public bool UseDefaultSignature
        {
            get => _convertingSettings.UseDefaultSignature;
            set => _convertingSettings.UseDefaultSignature = value;
        }
    }
}