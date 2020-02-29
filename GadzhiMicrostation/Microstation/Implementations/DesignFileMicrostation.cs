using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
using GadzhiMicrostation.Models.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Текущий файл Microstation
    /// </summary>
    public class DesignFileMicrostation : IDesignFileMicrostation
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly DesignFile _designFile;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        public DesignFileMicrostation(DesignFile designFile,
                                      IApplicationMicrostation applicationMicrostation,
                                      IMicrostationProject microstationProject)
        {
            _designFile = designFile;
            ApplicationMicrostation = applicationMicrostation;
            _microstationProject = microstationProject;
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => _designFile?.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDesingFileValid => _designFile != null;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        public IList<IModelMicrostation> ModelsMicrostation
        {
            get
            {
                List<IModelMicrostation> modelsMicrostation = new List<IModelMicrostation>();
                foreach (ModelReference model in _designFile.Models)
                {

                    modelsMicrostation.Add(new ModelMicrostation(model, ApplicationMicrostation));
                }
                return modelsMicrostation;
            }
        }

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IList<IStamp> Stamps => ModelsMicrostation.SelectMany(model => model.FindStamps()).
                                                          ToList();

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

        /// <summary>
        /// Найти все доступные штампы во всех моделях и листах. Начать обработку каждого из них
        /// </summary>       
        public IEnumerable<FileDataSourceMicrostation> CreatePdfInDesingFile(string filePath)
        {
            if (Stamps.Any())
            {
                return Stamps?.Select(stamp => CreatePdfWithSignatures(stamp, filePath));
            }
            else
            {
                ApplicationMicrostation.ErrorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.StampNotFound,
                                                     $"Штампы в файле {_microstationProject.FileDataMicrostation.FileName} не найдены"));
                return null;
            }
        }

        /// <summary>
        /// Создать файл типа DWG
        /// </summary>
        public void CreateDwgFile(string filePath)
        {
            _designFile.SaveAs(filePath, true, MsdDesignFileFormat.msdDesignFileFormatDWG);
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private FileDataSourceMicrostation CreatePdfWithSignatures(IStamp stamp, string filePath)
        {
            ApplicationMicrostation.LoggerMicrostation.ShowMessage($"Обработка штампа {stamp.Name}");
            stamp.CompressFieldsRanges();

            stamp.DeleteSignaturesPrevious();
            stamp.InsertSignatures();

            ApplicationMicrostation.LoggerMicrostation.ShowMessage($"Создание PDF для штампа {stamp.Name}");
            FileDataSourceMicrostation fileDataSourceMicrostation = CreatePdfByStamp(stamp, filePath);

            stamp.DeleteSignaturesInserted();

            return fileDataSourceMicrostation;
        }

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private FileDataSourceMicrostation CreatePdfByStamp(IStamp stamp, string filePath)
        {
            if (stamp != null)
            {
                CreatePdf(filePath, stamp.PaperSize, stamp.Range, stamp.Orientation, stamp.UnitScale,
                          _microstationProject.FileDataMicrostation.ColorPrint);

                return new FileDataSourceMicrostation(filePath, FileExtentionMicrostation.pdf, stamp.PaperSize,
                                                      _microstationProject.PrintersInformation.PdfPrinter.PrinterName);
            }
            else
            {
                throw new ArgumentNullException(nameof(stamp));
            }
        }

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private void CreatePdf(string filePath, string drawPaperSize, RangeMicrostation rangeToPrint, OrientationType orientation,
                              double printScale, ColorPrintMicrostation colorPrint)
        {
            bool isPrinterSet = ApplicationMicrostation.SetDefaultPrinter(_microstationProject.
                                                                          PrintersInformation.PdfPrinter);
            bool isFenceSet = ApplicationMicrostation.SetPrintingFenceByRange(rangeToPrint);
            ApplicationMicrostation.SetPrintingOrientation(orientation);
            bool isPaperSizeSet = ApplicationMicrostation.SetPrinterPaperSize(drawPaperSize, _microstationProject.
                                                                              PrintersInformation.PdfPrinter.PrefixSearchPaperSize);
            ApplicationMicrostation.SetPrintScale(printScale);
            ApplicationMicrostation.SetPrintColor(colorPrint);

            if (isPrinterSet && isFenceSet && isPaperSizeSet)
            {
                ApplicationMicrostation.PrintPdfCommand(filePath);
            }
        }
    }
}
