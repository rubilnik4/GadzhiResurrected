using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using GadzhiMicrostation.Extentions.StringAdditional;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Печать Microstation
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationLibraryPrinting
    {
        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        public IResultApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize) =>      
            new ResultApplication().
            WhereMap(result => stamp != null && stamp is IStampMicrostation)?.
            ConcatResult(SetPrintingFenceByRange(((IStampMicrostation)stamp).StampCellElement.Range))?.
            ConcatResult(SetPrinterPaperSize(stamp.PaperSize, prefixSearchPaperSize))?.
            Map(result =>
                {
                    SetPrintingOrientation(stamp.Orientation);
                    SetPrintScale(((IStampMicrostation)stamp).StampCellElement.UnitScale);
                    SetPrintColor(colorPrint);
                    if (result.OkStatus) PrintCommand();
                    return result;
                })
            ?? new ErrorApplication(ErrorApplicationType.StampNotFound, "Штамп не найден или не соответствует формату Microstation").
               ToResultConverting();

        /// <summary>
        /// Команда печати
        /// </summary>
        private void PrintCommand()
        {
            Application.CadInputQueue.SendCommand("PRINT EXECUTE");
        }

        /// <summary>
        /// Установить тип поворота
        /// </summary>       
        private void SetPrintingOrientation(OrientationType orientation)
        {
            if (orientation == OrientationType.Landscape)
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
        private IResultApplication SetPrintingFenceByRange(RangeMicrostation rangeToPrint)
        {
            if (rangeToPrint?.IsValid == true)
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

                return new ResultApplication();
            }
            else
            {
                return new ErrorApplication(ErrorApplicationType.RangeNotValid, "Диапазон печати задан некорректно").
                       ToResultConverting();
            }
        }

        /// <summary>
        /// Установить формат печати характерный для принтера
        /// </summary>       
        private IResultApplication SetPrinterPaperSize(string drawSize, string prefixSearchPaperSize) =>
            GetPrinterPaperSize(drawSize, prefixSearchPaperSize).
            WhereMap(paperName => !String.IsNullOrEmpty(paperName))?.
            Map(paperName =>
            {
                Application.CadInputQueue.SendKeyin("print driver printer.pltcfg");
                Application.CadInputQueue.SendCommand("Print Units mm");
                Application.CadInputQueue.SendCommand($"PRINT papername {paperName}");
                Application.CadInputQueue.SendCommand("PRINT MAXIMIZE");

                return new ResultApplication();
            })
            ?? new ErrorApplication(ErrorApplicationType.RangeNotValid, $"Формат печати {drawSize} не найден").
                   ToResultConverting();

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
        private void SetPrintColor(ColorPrintApplication colorPrint)
        {
            string colorCommand = String.Empty;
            switch (colorPrint)
            {
                case ColorPrintApplication.BlackAndWhite:
                    colorCommand = "monochrome";
                    break;
                case ColorPrintApplication.GrayScale:
                    colorCommand = "grayscale";
                    break;
                case ColorPrintApplication.Color:
                    colorCommand = "color";
                    break;
            }
            Application.CadInputQueue.SendCommand($"PRINT colormode {colorCommand}");
        }

        /// <summary>
        /// Получить формат принтера по формату штампа
        /// </summary>      
        private string GetPrinterPaperSize(string drawSize, string prefixSearchPaperSize) =>
            new PageSettings().
            Map(pageSettings => pageSettings.PrinterSettings.PaperSizes.Cast<PaperSize>().
                                             FirstOrDefault(paper => CheckPaperSizeName(paper.PaperName, drawSize, prefixSearchPaperSize))?.
                                             PaperName);

        /// <summary>
        /// Проверить соответсвие формата
        /// </summary>    
        private bool CheckPaperSizeName(string paperName, string drawPaperSize, string prefixSearchPaperSize)
        {
            bool success = false;

            if (paperName?.ContainsIgnoreCase(drawPaperSize) == true &&
                paperName?.ContainsIgnoreCase(prefixSearchPaperSize) == true)
            {
                int indexSubstringStart = paperName.IndexOf(drawPaperSize, StringComparison.OrdinalIgnoreCase);
                int indexSubstringEnd = indexSubstringStart + drawPaperSize.Length;

                if (indexSubstringEnd > paperName.Length)
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
            }
            return success;
        }
    }
}
