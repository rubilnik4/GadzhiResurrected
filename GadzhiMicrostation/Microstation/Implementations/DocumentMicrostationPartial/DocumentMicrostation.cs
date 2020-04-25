using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiMicrostation.Extentions.Microstation;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentLibrary
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly DesignFile _designFile;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        public DocumentMicrostation(DesignFile designFile, IApplicationMicrostation applicationMicrostation)
        {
            _designFile = designFile ?? throw new ArgumentNullException(nameof(designFile));
            ApplicationMicrostation = applicationMicrostation ?? throw new ArgumentNullException(nameof(applicationMicrostation));

            ModelsMicrostation = GetModelsMicrostation();
            StampContainer = new StampContainer(FindStamps(ModelsMicrostation), designFile.FullName);
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => _designFile?.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => _designFile != null;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        public IList<IModelMicrostation> ModelsMicrostation { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => _designFile.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void SaveAs(string filePath) => _designFile.SaveAs(filePath, true);

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        public void Close() => _designFile.Close();

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
                                       ToResultApplicationValue<IStampMicrostation>()).
            ResultValueOkBind(stamp => ApplicationMicrostation.SetPrinterPaperSize(stamp.PaperSize, prefixSearchPaperSize).
                                       ToResultApplicationValue<IStampMicrostation>())?.
            ResultVoidOk(stamp => ApplicationMicrostation.SetPrintingOrientation(stamp.Orientation)).
            ResultVoidOk(stamp => ApplicationMicrostation.SetPrintScale(stamp.StampCellElement.UnitScale)).
            ResultVoidOk(_ => ApplicationMicrostation.SetPrintColor(colorPrint)).
            ResultVoidOk(_ => ApplicationMicrostation.PrintCommand()).
            ToResultApplication();

        /// <summary>
        /// Экспорт файла в Dwg
        /// </summary>      
        public string Export(string filePath) =>
           (Path.GetFileNameWithoutExtension(filePath) + "." + FileExtentionMicrostation.dwg.ToString()).
           Map(fileName => Path.Combine(Path.GetDirectoryName(filePath), fileName)).
           Void(dwgFilePath => _designFile.SaveAs(dwgFilePath, true, MsdDesignFileFormat.msdDesignFileFormatDWG));

        /// <summary>
        /// Получить модели в текущем файле
        /// </summary>       
        private IList<IModelMicrostation> GetModelsMicrostation() =>
            _designFile.Models.ToIEnumerable().
            Select(model => new ModelMicrostation(model, ApplicationMicrostation)).
            Cast<IModelMicrostation>().
            ToList();       
    }
}
