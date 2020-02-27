using GadzhiMicrostation.Extensions.StringAdditional;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    public partial class ApplicationMicrostation : IApplicationMicrostationCommands
    {

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public ICellElementMicrostation CreateCellElementFromLibrary(string cellName,
                                                                     PointMicrostation origin,
                                                                     IModelMicrostation modelMicrostation,
                                                                     Action<ICellElementMicrostation> additionalParametrs = null)
        {
            if (!String.IsNullOrEmpty(cellName) && IsCellContainsInLibrary(cellName))
            {
                CellElement cellElement = _application.CreateCellElement2(cellName,
                                                            _application.Point3dFromXY(origin.X, origin.Y),
                                                            _application.Point3dFromXY(1, 1),
                                                            false,
                                                            _application.Matrix3dIdentity());

                var cellElementMicrostation = new CellElementMicrostation(cellElement, modelMicrostation?.ToOwnerContainerMicrostation());
                additionalParametrs?.Invoke(cellElementMicrostation);

                _application.ActiveDesignFile.Models[modelMicrostation.IdName].AddElement((Element)cellElement);

                return cellElementMicrostation;
            }
            else
            {
                ErrorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.SignatureNotFound,
                                                                       $"Идентефикатор библиотечного элемента не задан"));
                return null;
            }
        }

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public ICellElementMicrostation CreateSignatureFromLibrary(string cellName,
                                                                   PointMicrostation origin,
                                                                   IModelMicrostation modelMicrostation,
                                                                   Action<ICellElementMicrostation> additionalParametrs = null,
                                                                   string cellDescription = null)
        {
            AttachLibrary(StampAdditionalParameters.SignatureLibraryPath);
           
            string cellNameOriginalyOrFoundByDescription = ChangeCellNameByDescriptionIfNotFoundInLibrary(cellName, cellDescription);
            var cellElementMicrostation = CreateCellElementFromLibrary(cellNameOriginalyOrFoundByDescription, origin, modelMicrostation, additionalParametrs);

            DetachLibrary();

            return cellElementMicrostation;
        }

        /// <summary>
        /// Подключить библиотеку
        /// </summary>      
        private void AttachLibrary(string libraryPath)
        {
            if (_fileSystemOperationsMicrostation.IsFileExist(libraryPath))
            {
                _application.CadInputQueue.SendCommand("ATTACH LIBRARY " + libraryPath);
            }
            else
            {
                ErrorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.FileNotFound,
                                                                        $"Файл библиотеки {libraryPath} не найден"));
            }
        }

        /// <summary>
        /// Отключить библиотеку
        /// </summary>      
        private void DetachLibrary()
        {
            _application.CadInputQueue.SendCommand("DETACH LIBRARY ");
        }

        /// <summary>
        /// Замена имени ячейки по описанию в случае отсутствия в библиотеке
        /// </summary>       
        private string ChangeCellNameByDescriptionIfNotFoundInLibrary(string cellName, string cellDescription)
        {
            string originallyOrChangedCellName = null;
            if (IsCellContainsInLibrary(cellName))
            {
                originallyOrChangedCellName = cellName;
            }
            else if (!IsCellContainsInLibrary(cellName) && !String.IsNullOrEmpty(cellDescription))
            {
                originallyOrChangedCellName = FindCellNameByDescription(cellDescription) ?? cellName;
            }

            return originallyOrChangedCellName;
        }
        /// <summary>
        /// Содержится ли текущая ячейка в библиотеке 
        /// </summary>
        private bool IsCellContainsInLibrary(string cellName)
        {
            if (String.IsNullOrEmpty(cellName))
            {
                CellInformation cellInformation;
                CellInformationEnumerator сellInformationEnumerator = Application.GetCellInformationEnumerator(false, false);
                сellInformationEnumerator.Reset();
                while (сellInformationEnumerator.MoveNext())
                {
                    cellInformation = сellInformationEnumerator.Current;
                    if (cellInformation.Name.Equals(cellName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Содержится ли текущая ячейка в библиотеке 
        /// </summary>
        private string FindCellNameByDescription(string cellDescription)
        {
            CellInformation cellInformation;
            CellInformationEnumerator сellInformationEnumerator = Application.GetCellInformationEnumerator(false, false);
            сellInformationEnumerator.Reset();
            while (сellInformationEnumerator.MoveNext())
            {
                cellInformation = сellInformationEnumerator.Current;
                if (cellInformation.Description.ContainsIgnoreCase(cellDescription))
                {
                    return cellInformation.Name;
                }
            }
            return null;
        }
    }
}
