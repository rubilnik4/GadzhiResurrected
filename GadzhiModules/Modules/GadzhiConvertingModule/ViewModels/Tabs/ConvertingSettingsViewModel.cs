using System;
using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using Nito.Mvvm;
using Prism.Commands;

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

            PersonSignatures = NotifyTask.Create(applicationGadzhi.GetSignaturesNames());

            UpdateCommand = new DelegateCommand(Update);
        }

        public DelegateCommand UpdateCommand { get; }

        private void Update()
        {
            PersonSignature = _convertingSettings.PersonSignature;
            RaisePropertyChanged(nameof(PersonSignature));
        }
        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Параметрюшки";

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
            set => _convertingSettings.PersonSignature = value;
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
            set => _convertingSettings.PdfNamingType = value;
        }

        /// <summary>
        /// Принципы именования PDF
        /// </summary>
        public IReadOnlyDictionary<PdfNamingType, string> PdfNamingTypes => PdfNamingConverter.PdfNamingString;
    }
}