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
    public partial class ApplicationMicrostation: IApplicationMicrostationDesingFile
    {

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public ICellElementMicrostation CreateCellElementFromLibrary(string cellName,
                                                                     PointMicrostation origin,
                                                                     IModelMicrostation modelMicrostation,
                                                                     Action<ICellElementMicrostation> additionalParametrs = null)
        {
            if (!String.IsNullOrEmpty(cellName))
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
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.ArgumentNullReference,
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
                                                                   Action<ICellElementMicrostation> additionalParametrs = null)
        {
            AttachLibrary(StampAdditionalParameters.SignatureLibraryPath);
            var cellElementMicrostation = CreateCellElementFromLibrary(cellName, origin, modelMicrostation, additionalParametrs);
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
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.FileNotFound,
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
    }
}
