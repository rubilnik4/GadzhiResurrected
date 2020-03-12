using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Helpers.Implementations;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.Printers;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;

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
        public void PrintCommand()
        {
            Application.CadInputQueue.SendCommand("PRINT EXECUTE");
        }

        /// <summary>
        /// Установить тип поворота
        /// </summary>       
        private void SetPrintingOrientation(OrientationType orientation)
        {
            if (orientation == OrientationType.Horizontal)
            {
                Application.CadInputQueue.SendCommand("PRINT orientation landscape");
            }
            else
            {
                Application.CadInputQueue.SendCommand("PRINT orientation portrait");
            }
        }

        /// <summary>
        /// Установить границы печати по рамке
        /// </summary>
        private ErrorMicrostation SetPrintingFenceByRange(RangeMicrostation rangeToPrint)
        {
            if (rangeToPrint != null && rangeToPrint.IsValid)
            {
                var rangeInPoints = rangeToPrint?.ToPointsMicrostation().
                                                      Select(point => point.ToPoint3d()).
                                                      ToArray();

                ShapeElement shapeElement = Application.CreateShapeElement1(null, rangeInPoints, MsdFillMode.msdFillModeNotFilled);

                Fence fence = Application.ActiveDesignFile.Fence;
                fence.Undefine();

                Application.CadInputQueue.SendKeyin("view on 1");
                View view = Application.ActiveDesignFile.Views[1];

                fence.DefineFromElement(view, (Element)shapeElement);

                Application.CadInputQueue.SendKeyin("print boundary fence");

                return null;
            }
            else
            {
                return new ErrorMicrostation(ErrorMicrostationType.RangeNotValid, "Диапазон печати задан некорректно");
            }
        }

        /// <summary>
        /// Установить формат печати характерный для принтера
        /// </summary>       
        private ErrorMicrostation SetPrinterPaperSize(string drawSize, string prefixSearchPaperSize)
        {
            string paperNameFound = GetPrinterPaperSize(drawSize, prefixSearchPaperSize);

            if (!String.IsNullOrEmpty(paperNameFound))
            {
                Application.CadInputQueue.SendKeyin("print driver printer.pltcfg");
                Application.CadInputQueue.SendCommand("Print Units mm");
                Application.CadInputQueue.SendCommand($"PRINT papername {paperNameFound}");
                Application.CadInputQueue.SendCommand("PRINT MAXIMIZE");

                return null;
            }
            else
            {
                return new ErrorMicrostation(ErrorMicrostationType.RangeNotValid, $"Формат печати {drawSize} не найден");
            }

        }

        /// <summary>
        /// Установить масштаб печати
        /// </summary>       
        private void SetPrintScale(double paperScale)
        {
            string paperScaleString = paperScale.ToString(CultureInfo.CurrentCulture).
                                                 Replace(',', '.');
            Application.CadInputQueue.SendKeyin($"print scale {paperScaleString}");
        }

        /// <summary>
        /// Установить цвет печати
        /// </summary>       
        private void SetPrintColor(ColorPrintMicrostation colorPrint)
        {
            string colorCommand = String.Empty;
            switch (colorPrint)
            {
                case ColorPrintMicrostation.BlackAndWhite:
                    colorCommand = "monochrome";
                    break;
                case ColorPrintMicrostation.GrayScale:
                    colorCommand = "grayscale";
                    break;
                case ColorPrintMicrostation.Color:
                    colorCommand = "color";
                    break;
            }
            Application.CadInputQueue.SendCommand($"PRINT colormode {colorCommand}");
        }

        private string GetPrinterPaperSize(string drawFormat, string prefixSearchPaperSize)
        {
            var pageSettings = new PageSettings();
            string paperNameFound = pageSettings.PrinterSettings.PaperSizes.
                                                 Cast<PaperSize>().
                                                 FirstOrDefault(paper => CheckPaperName(paper.PaperName, drawFormat, prefixSearchPaperSize))?.
                                                 PaperName;
            return paperNameFound;
        }
        /// <summary>
        /// Проверить соответсвие формата
        /// </summary>    
        private bool CheckPaperName(string paperName, string drawPaperSize, string prefixSearchPaperSize)
        {
            bool success = false;

            if (!String.IsNullOrEmpty(paperName) && !String.IsNullOrEmpty(drawPaperSize) &&
                paperName.IndexOf(drawPaperSize, StringComparison.OrdinalIgnoreCase) > -1)
            {
                if (prefixSearchPaperSize == null ||
                    paperName.IndexOf(prefixSearchPaperSize, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    int indexSubstringStart = paperName.IndexOf(drawPaperSize, StringComparison.OrdinalIgnoreCase);
                    int indexSubstringEnd = indexSubstringStart + drawPaperSize.Length;

                    if (indexSubstringEnd > paperName.Length)
                    {
                        //дальше символов нет
                        success = true;
                    }
                    else
                    {
                        char postDrawFormat = paperName[indexSubstringEnd];
                        if (!Char.IsNumber(postDrawFormat) &&
                            postDrawFormat != 'x' &&
                             postDrawFormat != 'х')
                        {
                            //не число и не содержит знаков
                            success = true;
                        }
                    }
                }
            }
            return success;
        }
    }
}
