using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
using GadzhiMicrostation.Models.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Файл файл Microstation
    /// </summary>
    public class DocumentMicrostation : IDocumentLibrary
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly DesignFile _designFile;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationLibrary ApplicationMicrostation { get; }

        public DocumentMicrostation(DesignFile designFile,
                                    IApplicationLibrary applicationMicrostation)
        {
            _designFile = designFile;
            ApplicationMicrostation = applicationMicrostation;
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
        public IList<IStampMicrostation> Stamps => ModelsMicrostation.SelectMany(model => model.FindStamps()).ToList();

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
        /// Экспорт файла в другие форматы
        /// </summary>      
        public string Export(string filePath) => CreateDwgFile(filePath);       

        /// <summary>
        /// Создать файл типа DWG
        /// </summary>
        private string CreateDwgFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath) + "." + FileExtentionMicrostation.dwg.ToString();
            string dwgFilePath = Path.Combine(Path.GetDirectoryName(filePath), fileName);
            _designFile.SaveAs(dwgFilePath, true, MsdDesignFileFormat.msdDesignFileFormatDWG);
            
            return dwgFilePath;
        }       

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        //private FileDataSourceMicrostation CreatePdfByStamp(IStampMicrostation stamp, string filePath)
        //{
        //    if (stamp != null)
        //    {
        //        bool isPdfCreated = CreatePdf(filePath, stamp.PaperSize, stamp.Range, stamp.Orientation, stamp.UnitScale,
        //                                      _microstationProject.FileDataMicrostation.ColorPrint);
        //        if (isPdfCreated)
        //        {
        //            return new FileDataSourceMicrostation(filePath, FileExtentionMicrostation.pdf, stamp.PaperSize,
        //                                                                 _microstationProject.PrintersInformation.PdfPrinter.PrinterName);
        //        }
        //        return null;
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException(nameof(stamp));
        //    }
        //}

      
    }
}
