using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Печать Microstation
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати
        /// </summary>
        public void PrintCommand() => Application.CadInputQueue.SendCommand("PRINT EXECUTE");

        /// <summary>
        /// Установить тип поворота
        /// </summary>       
        public void SetPrintingOrientation(StampOrientationType orientation) =>
             Application.CadInputQueue.SendCommand($"PRINT orientation {orientation.ToString().ToLowerCaseCurrentCulture()}");

        /// <summary>
        /// Установить границы печати по рамке
        /// </summary>
        public IResultApplication SetPrintingFenceByRange(RangeMicrostation rangeToPrint) =>
            new ResultAppValue<RangeMicrostation>(rangeToPrint, new ErrorApplication(ErrorApplicationType.RangeNotValid, "Диапазон печати задан некорректно")).
            ResultValueOk(rangePrint => rangeToPrint.ToPointsMicrostation().
                                        Select(point => point.ToPoint3d()).
                                        ToArray()).
            ResultValueOk(rangeInPoints => Application.CreateShapeElement1(null, rangeInPoints, MsdFillMode.msdFillModeNotFilled)).
            ResultVoidOk(shape =>
            {
                var fence = Application.ActiveDesignFile.Fence;
                fence.Undefine();

                Application.CadInputQueue.SendKeyin("view on 1");
                var view = Application.ActiveDesignFile.Views[1];

                fence.DefineFromElement(view, (Element)shape);

                Application.CadInputQueue.SendKeyin("print boundary fence");
            }).ToResultApplication();

        /// <summary>
        /// Установить формат печати характерный для принтера
        /// </summary>       
        public IResultApplication SetPrinterPaperSize(StampPaperSizeType paperSize, string prefixSearchPaperSize) =>
            GetPrinterPaperSize(paperSize, prefixSearchPaperSize).
            ResultVoidOk(paperName =>
            {
                Application.CadInputQueue.SendKeyin("print driver printer.pltcfg");
                Application.CadInputQueue.SendCommand("Print Units mm");
                Application.CadInputQueue.SendCommand($"PRINT papername {paperName}");
                Application.CadInputQueue.SendCommand("PRINT MAXIMIZE");
            }).ToResultApplication();

        /// <summary>
        /// Установить масштаб печати
        /// </summary>       
        public void SetPrintScale(double paperScale)
        {
            string paperScaleString = paperScale.ToString(CultureInfo.CurrentCulture).Replace(',', '.');
            Application.CadInputQueue.SendKeyin($"print scale {paperScaleString}");
        }

        /// <summary>
        /// Установить цвет печати
        /// </summary>       
        public void SetPrintColor(ColorPrintApplication colorPrint)
        {
            void SetColorMode(string colorCommand) => Application.CadInputQueue.SendCommand($"PRINT colormode {colorCommand}");
            switch (colorPrint)
            {
                case ColorPrintApplication.BlackAndWhite:
                    SetColorMode("monochrome");
                    return;
                case ColorPrintApplication.GrayScale:
                    SetColorMode("grayscale");
                    return;
                case ColorPrintApplication.Color:
                    SetColorMode("color");
                    return;
                default:
                    throw new InvalidEnumArgumentException(nameof(colorPrint), (int)colorPrint, typeof(ColorPrintApplication));
            }
        }

        /// <summary>
        /// Получить формат принтера по формату штампа
        /// </summary>      
        private static IResultAppValue<string> GetPrinterPaperSize(StampPaperSizeType paperSize, string prefixSearchPaperSize) =>
            new PageSettings().
            Map(pageSettings => pageSettings.PrinterSettings.PaperSizes.Cast<PaperSize>()).
            FirstOrDefault(paper => CheckPaperSizeName(paper.PaperName, paperSize, prefixSearchPaperSize)).
            Map(paperSizeChecked => new ResultAppValue<string>(paperSizeChecked?.PaperName, new ErrorApplication(ErrorApplicationType.PaperSizeNotFound,
                                                                                                                $"Формат печати {paperSize} не найден")));

        /// <summary>
        /// Проверить соответствие формата
        /// </summary>    
        private static bool CheckPaperSizeName(string paperName, StampPaperSizeType paperSize, string prefixSearchPaperSize)
        {
            string drawPaperSize = paperSize.ToString();
            if (paperName?.ContainsIgnoreCase(drawPaperSize) != true ||
                paperName.ContainsIgnoreCase(prefixSearchPaperSize) != true) return false;

            int indexSubstringStart = paperName.IndexOf(drawPaperSize, StringComparison.OrdinalIgnoreCase);
            int indexSubstringEnd = indexSubstringStart + drawPaperSize.Length;

            bool success = false;
            if (indexSubstringEnd >= paperName.Length)
            {
                success = true; //дальше символов нет
            }
            else
            {
                char postDrawFormat = paperName[indexSubstringEnd];
                if (!Char.IsNumber(postDrawFormat) && postDrawFormat != 'x' && postDrawFormat != 'х')
                {
                    success = true; //не число и не содержит знаков
                }
            }
            return success;
        }
    }
}
