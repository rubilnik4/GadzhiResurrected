using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Helpers;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Extensions.StringAdditional;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    public partial class ApplicationMicrostation : IApplicationMicrostationCommands
    {
        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке и проверить наличие такой в библиотеке
        /// </summary>       
        public IResultAppValue<ICellElementMicrostation> CreateCellElementFromLibrary(IList<LibraryElement> libraryElements, string cellName, PointMicrostation origin,
                                                                                      IModelMicrostation modelMicrostation,
                                                                                      Func<ICellElementMicrostation, ICellElementMicrostation> additionalParameters = null,
                                                                                      string cellDescription = null) =>
            ChangeCellNameByDescriptionIfNotFoundInLibrary(libraryElements, StringIdPrepare(cellName), cellDescription).
            ResultValueOk(cellNameChanged => CreateCellElementWithoutCheck(cellNameChanged, origin, modelMicrostation, additionalParameters));

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public IResultAppValue<ICellElementMicrostation> CreateSignatureFromLibrary(string cellName, PointMicrostation origin,
                                                                                    IModelMicrostation modelMicrostation,
                                                                                    Func<ICellElementMicrostation, ICellElementMicrostation> additionalParameters = null,
                                                                                    string cellDescription = null) =>
            AttachLibrary(MicrostationResources.SignatureMicrostationFileName).
            ResultValueOkBind(libraryElements => CreateCellElementFromLibrary(libraryElements, cellName, origin, modelMicrostation, additionalParameters)).
            ResultVoidOk(_ => DetachLibrary());

        /// <summary>
        /// Подключить библиотеку
        /// </summary>      
        public IResultAppCollection<LibraryElement> AttachLibrary(string libraryPathMicrostation) =>
            new ResultAppValue<string>(libraryPathMicrostation, new ErrorApplication(ErrorApplicationType.FileNotFound,
                                                                          "Не задан путь к библиотеке подписей Microstation")).
            ResultVoidOk(libraryPath => _application.CadInputQueue.SendCommand("ATTACH LIBRARY " + libraryPath)).
            ResultValueOkBind(_ => new ResultAppCollection<LibraryElement>(CachingLibraryElements()))
            as IResultAppCollection<LibraryElement>;

        /// <summary>
        /// Отключить библиотеку
        /// </summary>      
        public void DetachLibrary() => _application.CadInputQueue.SendCommand("DETACH LIBRARY ");

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        private ICellElementMicrostation CreateCellElementWithoutCheck(string cellName, PointMicrostation origin,
                                                                       IModelMicrostation modelMicrostation,
                                                                       Func<ICellElementMicrostation, ICellElementMicrostation> additionalParameters = null)
        {
            var cellElement = _application.CreateCellElement2(StringIdPrepare(cellName),
                                                                      _application.Point3dFromXY(origin.X, origin.Y),
                                                                      _application.Point3dFromXY(1, 1),
                                                                      false, _application.Matrix3dIdentity());

            var cellDefaultOrigin = new CellElementMicrostation(cellElement, modelMicrostation.ToOwnerMicrostation());
            var cellElementMicrostation = additionalParameters?.Invoke(cellDefaultOrigin) ?? cellDefaultOrigin;

            _application.ActiveDesignFile.Models[modelMicrostation.IdName].AddElement((Element)cellElement);

            return cellElementMicrostation;
        }

        /// <summary>
        /// Замена имени ячейки по описанию в случае отсутствия в библиотеке
        /// </summary>       
        private static IResultAppValue<string> ChangeCellNameByDescriptionIfNotFoundInLibrary(IList<LibraryElement> libraryElements,
                                                                                              string cellName, string cellDescription) =>
            new ResultAppValue<string>(cellName, new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Отсутствует Id {cellName} подписи")).
            ResultValueContinue(nameInLibrary => !String.IsNullOrEmpty(cellName) && IsCellContainsInLibrary(libraryElements, nameInLibrary),
                okFunc: nameInLibrary => nameInLibrary,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись по Id {cellName} не найдена")).
            ResultValueBadBind(_ => FindCellNameByDescription(libraryElements, cellDescription)).
            ResultValueBadBind(_ => GetCellNameRandom(libraryElements));


        /// <summary>
        /// Кэшировать элементы
        /// </summary>
        private IEnumerable<LibraryElement> CachingLibraryElements()
        {
            var cellInformationEnumerator = Application.GetCellInformationEnumerator(false, false);
            cellInformationEnumerator.Reset();

            // var cachingLibraryElements = new List<LibraryElement>();
            while (cellInformationEnumerator.MoveNext())
            {
                var cellInformation = cellInformationEnumerator.Current;
                yield return new LibraryElement(cellInformation.Name, cellInformation.Description);
            }
        }

        /// <summary>
        /// Содержится ли текущая ячейка в библиотеке 
        /// </summary>       
        private static bool IsCellContainsInLibrary(IEnumerable<LibraryElement> libraryElements, string cellName) =>
            libraryElements.Any(libraryElement => libraryElement.Name == cellName);

        /// <summary>
        /// Подготовить строку для поиска в библиотеке
        /// </summary>
        private static string StringIdPrepare(string name) => name?.Trim('{', '}') ?? String.Empty;

        /// <summary>
        /// Найти замену имени ячейки по описанию
        /// </summary>
        private static IResultAppValue<string> FindCellNameByDescription(IEnumerable<LibraryElement> libraryElements, 
                                                                         string cellDescription) =>
            libraryElements.
            FirstOrDefault(libraryElement => libraryElement.Description.ContainsIgnoreCase(cellDescription)).
            Map(libraryElement => new ResultAppValue<string>(libraryElement.Name))
            ?? new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись по фамилии {cellDescription} не найдена").
               Map(error => new ResultAppValue<string>(error));

        /// <summary>
        /// Вернуть случайное имя
        /// </summary>
        private static IResultAppValue<string> GetCellNameRandom(IList<LibraryElement> libraryElements) =>
            libraryElements.
            Map(cache => cache[RandomInstance.RandomNumber(cache.Count)].Name).
            Map(name => new ResultAppValue<string>(name));
    }
}
