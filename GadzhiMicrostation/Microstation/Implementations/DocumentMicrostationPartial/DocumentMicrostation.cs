using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.DocumentMicrostationPartial
{
    /// <summary>
    /// Файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentLibrary
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly Application _application;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        public DocumentMicrostation(Application application, IApplicationMicrostation applicationMicrostation)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
            ApplicationMicrostation = applicationMicrostation ?? throw new ArgumentNullException(nameof(applicationMicrostation));

            ModelsMicrostation = GetModelsMicrostation();
            StampContainer = new StampContainer(FindStamps(ModelsMicrostation), DesignFile.FullName);
        }

        /// <summary>
        /// Текущий документ
        /// </summary>
        private DesignFile DesignFile => _application.ActiveDesignFile;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => DesignFile?.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => DesignFile != null;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        public IList<IModelMicrostation> ModelsMicrostation { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => DesignFile.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void SaveAs(string filePath) => DesignFile.SaveAs(filePath, true);

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
        public string Export(string filePath) =>
           (Path.GetFileNameWithoutExtension(filePath) + "." + FileExtensionMicrostation.Dwg).
           Map(fileName => Path.Combine(Path.GetDirectoryName(filePath) ?? String.Empty, fileName)).
           Void(dwgFilePath => DesignFile.SaveAs(dwgFilePath, true, MsdDesignFileFormat.msdDesignFileFormatDWG));

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
