using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.DocumentMicrostationPartial
{
    /// <summary>
    /// Файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentMicrostation
    {
        public DocumentMicrostation(Application application, IApplicationMicrostation applicationMicrostation)
        {
            _application = application;
            ApplicationMicrostation = applicationMicrostation;
        }

        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly Application _application;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Текущий документ
        /// </summary>
        private DesignFile DesignFile => _application.ActiveDesignFile;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _fullName;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => _fullName ??= DesignFile.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => DesignFile != null;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        private IList<IModelMicrostation> _modelsMicrostation;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        public IList<IModelMicrostation> ModelsMicrostation => _modelsMicrostation ??= GetModelsMicrostation();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => DesignFile.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public IResultApplication SaveAs(string filePath) =>
            Path.GetExtension(filePath).
            WhereContinue(fileExtension => ValidMicrostationExtensions.IsFileExtensionEqual(fileExtension, FileExtensionMicrostation.Dgn),
            okFunc: fileExtension => new ResultApplication().
                                     ResultVoidOk(_ => DesignFile.SaveAs(filePath, true)).
                                     ToResultApplication(),
            badFunc: fileExtension => new ResultApplication(new ErrorApplication(ErrorApplicationType.IncorrectExtension,
                                                                                 $"Некорректное расширение {fileExtension} для файла типа dgn")));

        /// <summary>
        /// Подключить дополнительные файлы
        /// </summary>
        public void AttachAdditional() =>
            ApplicationMicrostation.AttachAdditional(FullName);
        

        /// <summary>
        /// Отключить дополнительные файлы
        /// </summary>
        public void DetachAdditional() =>
            ApplicationMicrostation.DetachAdditional();

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        public void Close() => DesignFile.Close();

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        public void CloseWithSaving()
        {
            Save();
            Close();
        }

        public void CloseApplication() => ApplicationMicrostation.CloseApplication();

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        public IResultApplication PrintStamp(IStamp stampContainer, ColorPrintApplication colorPrint, string prefixSearchPaperSize) =>
            new ResultAppValue<IStamp>(stampContainer, new ErrorApplication(ErrorApplicationType.StampNotFound, "Штамп не найден")).
            ResultValueContinue(stamp => stamp is IStampMicrostation,
                okFunc: stamp => (IStampMicrostation)stamp,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.StampNotFound, "Штамп не соответствует формату Microstation")).
            ResultVoidOk(stamp => stamp.StampCellElement.ModelMicrostation.Activate()).
            ResultValueOkBind(stamp => ApplicationMicrostation.SetPrintingFenceByRange(stamp.StampCellElement.Range).
                                       ToResultApplicationValue(stamp)).
            ResultValueOkBind(stamp => ApplicationMicrostation.SetPrinterPaperSize(stamp.PaperSize, prefixSearchPaperSize).
                                       ToResultApplicationValue(stamp))?.
            ResultVoidOk(stamp => ApplicationMicrostation.SetPrintingOrientation(stamp.Orientation)).
            ResultVoidOk(stamp => ApplicationMicrostation.SetPrintScale(stamp.StampCellElement.UnitScale)).
            ResultVoidOk(_ => ApplicationMicrostation.SetPrintColor(colorPrint)).
            ResultVoidOk(_ => ApplicationMicrostation.PrintCommand()).
            ToResultApplication();

        /// <summary>
        /// Экспорт файла в Dwg
        /// </summary>      
        public IResultAppValue<string> Export(string filePath, StampDocumentType stampDocumentType) =>
            Path.GetExtension(filePath).
            WhereContinue(fileExtension => ValidMicrostationExtensions.IsFileExtensionEqual(fileExtension, FileExtensionMicrostation.Dwg),
            okFunc: fileExtension => new ResultAppValue<string>(filePath).
                                     ResultVoidOk(_ => DesignFile.SaveAs(filePath, true, MsdDesignFileFormat.msdDesignFileFormatDWG)),
            badFunc: fileExtension => new ResultAppValue<string>(new ErrorApplication(ErrorApplicationType.IncorrectExtension,
                                                                                      $"Некорректное расширение {fileExtension} для файла типа dgn")));

        /// <summary>
        /// Получить модели в текущем файле
        /// </summary>       
        private IList<IModelMicrostation> GetModelsMicrostation() =>
            DesignFile.Models.ToIEnumerable().
            Select(model => new ModelMicrostation(model, ApplicationMicrostation)).
            Cast<IModelMicrostation>().
            ToList();
    }
}
