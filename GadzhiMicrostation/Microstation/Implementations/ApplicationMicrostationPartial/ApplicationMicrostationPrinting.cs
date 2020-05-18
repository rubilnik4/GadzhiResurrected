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
        public void SetPrintingOrientation(OrientationType orientation) =>
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
        public IResultApplication SetPrinterPaperSize(string drawSize, string prefixSearchPaperSize) =>
            GetPrinterPaperSize(drawSize, prefixSearchPaperSize).
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
        private static IResultAppValue<string> GetPrinterPaperSize(string drawSize, string prefixSearchPaperSize) =>
            new PageSettings().
            Map(pageSettings => pageSettings.PrinterSettings.PaperSizes.Cast<PaperSize>()).
            FirstOrDefault(paper => CheckPaperSizeName(paper.PaperName, PreparePaperSize(drawSize), prefixSearchPaperSize)).
            Map(paperSize => new ResultAppValue<string>(paperSize?.PaperName, new ErrorApplication(ErrorApplicationType.PaperSizeNotFound,
                                                                                                   $"Формат печати {drawSize} не найден")));

        /// <summary>
        /// Проверить соответствие формата
        /// </summary>    
        private static bool CheckPaperSizeName(string paperName, string drawPaperSize, string prefixSearchPaperSize)
        {
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

        /// <summary>
        /// Преобразовать формат бумаги к корректному виду
        /// </summary>
        private static string PreparePaperSize(string drawPaperSize) => 
            drawPaperSize?.Replace('х', 'x').Trim();
    }
}
