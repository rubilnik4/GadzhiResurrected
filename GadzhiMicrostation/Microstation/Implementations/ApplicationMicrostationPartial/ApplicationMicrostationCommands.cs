using GadzhiMicrostation.Extentions.StringAdditional;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using GadzhiMicrostation.Helpers;
using GadzhiApplicationCommon.Extensions.Functional;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    public partial class ApplicationMicrostation : IApplicationMicrostationCommands
    {
        /// <summary>
        /// Кэшированная библиотека элементов
        /// </summary>
        private IList<LibraryElement> _cachLibraryElements;

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке и проверить наличие такой в библиотеке
        /// </summary>       
        public ICellElementMicrostation CreateCellElementFromLibrary(string cellName, PointMicrostation origin,
                                                                     IModelMicrostation modelMicrostation,
                                                                     Action<ICellElementMicrostation> additionalParametrs = null,
                                                                     string cellDescription = null) =>
            ChangeCellNameByDescriptionIfNotFoundInLibrary(cellName, cellDescription).
            Map(cellNameChanged => !String.IsNullOrEmpty(cellNameChanged) ?
                                   CreateCellElementFromLibraryWithoutCheck(cellNameChanged, origin, modelMicrostation, additionalParametrs) :
                                   null);

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public ICellElementMicrostation CreateSignatureFromLibrary(string cellName, PointMicrostation origin,
                                                                   IModelMicrostation modelMicrostation,
                                                                   Action<ICellElementMicrostation> additionalParametrs = null,
                                                                   string cellDescription = null)
        {
            AttachLibrary(MicrostationResources.SignatureMicrostationFileName);

            var cellElementMicrostation = CreateCellElementFromLibrary(cellName, origin, modelMicrostation, additionalParametrs);

            DetachLibrary();

            return cellElementMicrostation;
        }

        /// <summary>
        /// Подключить библиотеку
        /// </summary>      
        public void AttachLibrary(string libraryPath)
        {
            _application.CadInputQueue.SendCommand("ATTACH LIBRARY " + libraryPath);
            _cachLibraryElements = CachingLibraryElements();
        }

        /// <summary>
        /// Отключить библиотеку
        /// </summary>      
        public void DetachLibrary()
        {
            _application.CadInputQueue.SendCommand("DETACH LIBRARY ");
            _cachLibraryElements = null;
        }


        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        private ICellElementMicrostation CreateCellElementFromLibraryWithoutCheck(string cellName, PointMicrostation origin,
                                                                                  IModelMicrostation modelMicrostation,
                                                                                  Action<ICellElementMicrostation> additionalParameters = null)
        {
            CellElement cellElement = _application.CreateCellElement2(cellName,
                                                                      _application.Point3dFromXY(origin.X, origin.Y),
                                                                      _application.Point3dFromXY(1, 1),
                                                                      false,
                                                                      _application.Matrix3dIdentity());

            var cellElementMicrostation = new CellElementMicrostation(cellElement, modelMicrostation?.ToOwnerMicrostation());
            additionalParameters?.Invoke(cellElementMicrostation);

            _application.ActiveDesignFile.Models[modelMicrostation.IdName].AddElement((Element)cellElement);

            return cellElementMicrostation;
        }

        /// <summary>
        /// Замена имени ячейки по описанию в случае отсутствия в библиотеке
        /// </summary>       
        private string ChangeCellNameByDescriptionIfNotFoundInLibrary(string cellName, string cellDescription)
        {
            string originallyOrChangedCellName = null;
            bool isCellContainsInLibrary = IsCellContainsInLibrary(cellName);
            if (isCellContainsInLibrary)
            {
                originallyOrChangedCellName = cellName;
            }
            else if (!isCellContainsInLibrary && !String.IsNullOrEmpty(cellDescription))
            {
                originallyOrChangedCellName = FindCellNameByDescription(cellDescription) ?? cellName;
            }
            else
            {
                originallyOrChangedCellName = GetCellNameRandom();
            }

            return originallyOrChangedCellName;
        }

        /// <summary>
        /// Кэшировать элементы
        /// </summary>
        private IList<LibraryElement> CachingLibraryElements()
        {
            CellInformation cellInformation;
            CellInformationEnumerator сellInformationEnumerator = Application.GetCellInformationEnumerator(false, false);
            сellInformationEnumerator.Reset();

            var cachingLibraryElements = new List<LibraryElement>();
            while (сellInformationEnumerator.MoveNext())
            {
                cellInformation = сellInformationEnumerator.Current;
                cachingLibraryElements.Add(new LibraryElement(cellInformation.Name, cellInformation.Description));
            }

            return cachingLibraryElements;
        }

        /// <summary>
        /// Содержится ли текущая ячейка в библиотеке 
        /// </summary>       
        private bool IsCellContainsInLibrary(string cellName)
        {
            if (!String.IsNullOrEmpty(cellName))
            {
                return _cachLibraryElements?.Any(libraryElement =>
                        libraryElement.Name.ContainsIgnoreCase(cellName)) == true;
            }
            return false;
        }

        /// <summary>
        /// Найти замену имени ячейки по описанию
        /// </summary>
        private string FindCellNameByDescription(string cellDescription) =>
           !String.IsNullOrEmpty(cellDescription) ?
            _cachLibraryElements?.FirstOrDefault(libraryElement =>
                                  libraryElement.Description.ContainsIgnoreCase(cellDescription)).Name :
            null;


        /// <summary>
        /// Вернуть случайное имя
        /// </summary>
        private string GetCellNameRandom() =>
            _cachLibraryElements?.
            Map(cach => cach[RandomInstance.RandomNumber(cach.Count)].Name);      
    }
}
