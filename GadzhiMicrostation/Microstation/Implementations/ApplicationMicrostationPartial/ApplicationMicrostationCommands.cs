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
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;

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
        public IResultApplicationValue<ICellElementMicrostation> CreateCellElementFromLibrary(string cellName, PointMicrostation origin,
                                                                     IModelMicrostation modelMicrostation,
                                                                     Func<ICellElementMicrostation, ICellElementMicrostation> additionalParametrs = null,
                                                                     string cellDescription = null) =>
            ChangeCellNameByDescriptionIfNotFoundInLibrary(cellName, cellDescription).
            ResultValueOk(cellNameChanged => CreateCellElementWithoutCheck(cellNameChanged, origin, modelMicrostation, additionalParametrs));

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public IResultApplicationValue<ICellElementMicrostation> CreateSignatureFromLibrary(string cellName, PointMicrostation origin,
                                                                   IModelMicrostation modelMicrostation,
                                                                   Func<ICellElementMicrostation, ICellElementMicrostation> additionalParametrs = null,
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
        private ICellElementMicrostation CreateCellElementWithoutCheck(string cellName, PointMicrostation origin,
                                                                                  IModelMicrostation modelMicrostation,
                                                                                  Func<ICellElementMicrostation, ICellElementMicrostation> additionalParameters = null)
        {
            CellElement cellElement = _application.CreateCellElement2(cellName,
                                                                      _application.Point3dFromXY(origin.X, origin.Y),
                                                                      _application.Point3dFromXY(1, 1),
                                                                      false,_application.Matrix3dIdentity());

            var cellDefaultOrigin = new CellElementMicrostation(cellElement, modelMicrostation?.ToOwnerMicrostation());
            var cellElementMicrostation = additionalParameters?.Invoke(cellDefaultOrigin) ?? cellDefaultOrigin;

            _application.ActiveDesignFile.Models[modelMicrostation.IdName].AddElement((Element)cellElement);
            return cellElementMicrostation;
        }

        /// <summary>
        /// Замена имени ячейки по описанию в случае отсутствия в библиотеке
        /// </summary>       
        private IResultApplicationValue<string> ChangeCellNameByDescriptionIfNotFoundInLibrary(string cellName, string cellDescription) =>
            cellName?.
            WhereContinue(nameInLibrary => IsCellContainsInLibrary(nameInLibrary),
                okFunc: nameInLibrary => new ResultApplicationValue<string>(nameInLibrary),
                badFunc: _ => FindCellNameByDescription(cellDescription)).
            WhereBad(result => result.OkStatus,               
                badFunc: _ => FindCellNameByDescription(cellDescription)).
            WhereBad(result => result.OkStatus,
                badFunc: _ => GetCellNameRandom())
            ?? new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись {cellDescription} не найдена").
               ToResultApplicationValue<string>();

       
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
        private bool IsCellContainsInLibrary(string cellName) =>          
             _cachLibraryElements?.Any(libraryElement => libraryElement.Name.ContainsIgnoreCase(cellName)) == true;

        /// <summary>
        /// Найти замену имени ячейки по описанию
        /// </summary>
        private IResultApplicationValue<string> FindCellNameByDescription(string cellDescription) =>
            _cachLibraryElements?.
            FirstOrDefault(libraryElement => libraryElement.Description.ContainsIgnoreCase(cellDescription))?.
            Map(libraryElement => new ResultApplicationValue<string>(libraryElement.Name))
            ?? new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись по фамилии {cellDescription} не найдена").
               Map(error => new ResultApplicationValue<string>(error));

        /// <summary>
        /// Вернуть случайное имя
        /// </summary>
        private IResultApplicationValue<string> GetCellNameRandom() =>
            _cachLibraryElements?.
            Map(cach => cach[RandomInstance.RandomNumber(cach.Count)].Name).
            Map(name => new ResultApplicationValue<string>(name))
            ?? new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"База подписей не установлена").
               Map(error => new ResultApplicationValue<string>(error));
    }
}
