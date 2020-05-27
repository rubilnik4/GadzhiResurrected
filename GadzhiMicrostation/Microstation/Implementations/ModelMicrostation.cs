using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Converters;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public class ModelMicrostation : IModelMicrostation
    {
        /// <summary>
        /// Экземпляр модели или листа
        /// </summary>
        private readonly ModelReference _modelMicrostation;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        public ModelMicrostation(ModelReference modelMicrostation, IApplicationMicrostation applicationMicrostation)
        {
            _modelMicrostation = modelMicrostation ?? throw new ArgumentNullException(nameof(modelMicrostation));
            ApplicationMicrostation = applicationMicrostation ?? throw new ArgumentNullException(nameof(applicationMicrostation));
        }

        /// <summary>
        /// Порядковый идентификационный номер
        /// </summary>
        public string IdName => _modelMicrostation.Name;

        /// <summary>
        /// Коэффициенты преобразования координат в текущие
        /// </summary>
        private UnitsMicrostation UnitsMicrostation => new UnitsMicrostation(_modelMicrostation.get_MasterUnit().UnitsPerBaseNumerator,
                                                                            _modelMicrostation.get_MasterUnit().UnitsPerBaseDenominator,
                                                                            _modelMicrostation.get_SubUnit().UnitsPerBaseNumerator,
                                                                            _modelMicrostation.get_SubUnit().UnitsPerBaseDenominator,
                                                                            _modelMicrostation.UORsPerStorageUnit);

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public double UnitScale => UnitsMicrostation.Global;

        /// <summary>
        /// Выбрать текущую модель
        /// </summary>
        public void Activate() => _modelMicrostation.Activate();

        /// <summary>
        /// Найти элементы в модели по типу
        /// </summary>       
        public IEnumerable<IElementMicrostation> GetModelElementsMicrostation(ElementMicrostationType includeTypeMicrostation) =>
            GetModelElements(new List<ElementMicrostationType>() { includeTypeMicrostation }).
                   Select(element => ConvertMicrostationElements.ToMicrostationElement(element, ToOwnerMicrostation()));

        /// <summary>
        /// Найти элементы в модели по типам
        /// </summary>       
        public IEnumerable<IElementMicrostation> GetModelElementsMicrostation(IEnumerable<ElementMicrostationType> includeTypesMicrostation = null) =>
            GetModelElements(includeTypesMicrostation).
                   Select(element => ConvertMicrostationElements.ToMicrostationElement(element, ToOwnerMicrostation()));

        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        public IEnumerable<IStamp> FindStamps(int modelIndex, ConvertingSettingsApplication convertingSettings) =>
            GetModelElements(new List<ElementMicrostationType>() { ElementMicrostationType.CellElement }).
            Where(element => StampFieldMain.IsStampName(element.AsCellElement.Name)).
            Select((element, stampIndex) => 
                       new CellElementMicrostation(element.AsCellElement, ToOwnerMicrostation()).
                           Map(cellMicrostation => new StampMainMicrostation(cellMicrostation,
                                                                             new StampSettings(new StampIdentifier(modelIndex, stampIndex),
                                                                                               convertingSettings.PersonId, 
                                                                                               convertingSettings.PdfNamingType),
                                                                             ApplicationMicrostation.MicrostationResources.SignaturesLibrarySearching))).
            Cast<IStamp>();

        /// <summary>
        /// Найти элементы библиотеки Microstation в модели по типам
        /// </summary>       
        private IEnumerable<Element> GetModelElements(IEnumerable<ElementMicrostationType> includeTypesMicrostation)
        {
            var elementScanCriteria = new ElementScanCriteria();
            elementScanCriteria.ExcludeAllTypes();

            var includeTypes = includeTypesMicrostation?.Select(ConvertingElementMicrostationTypes.ToMsdMicrostation)
                               ?? Enumerable.Empty<MsdElementType>();

            foreach (var msdElementType in includeTypes)
            {
                elementScanCriteria.IncludeType(msdElementType);
            }

            var elementEnumerator = _modelMicrostation.Scan(elementScanCriteria);
            while (elementEnumerator.MoveNext())
            {
                yield return (Element)elementEnumerator.Current;
            }
        }

        /// <summary>
        /// Удалить элемент
        /// </summary>      
        public void RemoveElement(long id)
        {
            var element = _modelMicrostation.GetElementByID64(id);
            _modelMicrostation.RemoveElement(element);
        }

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerMicrostation ToOwnerMicrostation() => new OwnerMicrostation(this);
    }
}
