using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Models.Implementations;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    public partial class ApplicationMicrostation : IApplicationMicrostationCommands
    {
        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке и проверить наличие такой в библиотеке
        /// </summary>       
        public IResultAppValue<ICellElementMicrostation> CreateCellElementFromLibrary(string cellName, PointMicrostation origin,
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

            return new ResultAppValue<ICellElementMicrostation>(cellElementMicrostation,
                                                                new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Элемент подписи не найден"));
        }

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        public IResultAppValue<ICellElementMicrostation> CreateSignatureFromLibrary(string cellName, PointMicrostation origin,
                                                                                        IModelMicrostation modelMicrostation,
                                                                                        Func<ICellElementMicrostation, ICellElementMicrostation> additionalParameters = null) =>
                AttachLibrary(MicrostationResources.SignatureMicrostationFileName).
                ResultValueOkBind(libraryElements => CreateCellElementFromLibrary(cellName, origin, modelMicrostation, additionalParameters)).
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
        /// Изменить имя вложенных файлов
        /// </summary>
        public void ChangeAttachNameFiles(string filePath, string fileName)
        {
            var rasterManager = Application.RasterManager;
           if (rasterManager.Rasters.Count != 1) return;

            var raster = rasterManager.Rasters[1];
            var rasterPath = Path.GetDirectoryName(filePath) +  "\\" + Path.GetFileNameWithoutExtension(fileName) + 
                             Path.GetExtension(raster.RasterInformation.FullName);
            var rasterAttached = rasterManager.Rasters.Attach(rasterPath);
            rasterAttached.GeoReferenceInformation.set_Origin(raster.GeoReferenceInformation.get_Origin());
            rasterAttached.GeoReferenceInformation.set_Extent(raster.GeoReferenceInformation.get_Extent());
        }

        /// <summary>
        /// Кэшировать элементы
        /// </summary>
        private IEnumerable<LibraryElement> CachingLibraryElements()
        {
            var cellInformationEnumerator = Application.GetCellInformationEnumerator(false, false);
            cellInformationEnumerator.Reset();

            while (cellInformationEnumerator.MoveNext())
            {
                var cellInformation = cellInformationEnumerator.Current;
                yield return new LibraryElement(cellInformation.Name, cellInformation.Description);
            }
        }

        /// <summary>
        /// Подготовить строку для поиска в библиотеке
        /// </summary>
        private static string StringIdPrepare(string name) => name?.Trim('{', '}') ?? String.Empty;
    }
}
