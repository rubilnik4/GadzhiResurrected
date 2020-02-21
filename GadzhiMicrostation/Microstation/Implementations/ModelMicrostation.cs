using GadzhiMicrostation.Microstation.Converters;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Implementations.StampPartial;
using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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


        public ModelMicrostation(ModelReference modelMicrostation,
                                 IApplicationMicrostation applicationMicrostation)
        {
            _modelMicrostation = modelMicrostation;
            ApplicationMicrostation = applicationMicrostation;
        }

        /// <summary>
        /// Порядковый идентефикационный номер
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
        /// Найти элементы в модели по типу
        /// </summary>       
        public IEnumerable<IElementMicrostation> GetModelElementsMicrostation(ElementMicrostationType includeTypeMicrostation)
        {
            return GetModelElements(new List<ElementMicrostationType>() { includeTypeMicrostation }).
                   Select(element => ConvertMicrostationElements.ConvertToMicrostationElement(element, ToOwnerContainerMicrostation()));
        }

        /// <summary>
        /// Найти элементы в модели по типам
        /// </summary>       
        public IEnumerable<IElementMicrostation> GetModelElementsMicrostation(IEnumerable<ElementMicrostationType> includeTypesMicrostation = null)
        {
            return GetModelElements(includeTypesMicrostation).
                   Select(element => ConvertMicrostationElements.ConvertToMicrostationElement(element, ToOwnerContainerMicrostation()));
        }

        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        public IEnumerable<IStamp> FindStamps()
        {
            return GetModelElements(new List<ElementMicrostationType>() { ElementMicrostationType.CellElement }).
                   Cast<CellElement>().
                   Where(cellElement => StampMain.IsStampName(cellElement.Name)).
                   Select(cellElement => new Stamp(cellElement, ToOwnerContainerMicrostation())).
                   Cast<IStamp>();          
        }

        /// <summary>
        /// Найти элементы библиотеки Microstation в модели по типам
        /// </summary>       
        private IEnumerable<Element> GetModelElements(IEnumerable<ElementMicrostationType> includeTypesMicrostation = null)
        {
            if (_modelMicrostation != null)
            {
                var elementScanCriteria = new ElementScanCriteria();
                elementScanCriteria.ExcludeAllTypes();

                var includeTypes = includeTypesMicrostation?.Select(type => ConvertElementMicrostationTypes.ConvertToMsdMicrostation(type));
                if (includeTypes != null)
                {
                    foreach (MsdElementType type in includeTypes)
                    {
                        elementScanCriteria.IncludeType(type);
                    }
                }                

                ElementEnumerator elementEnumerator = _modelMicrostation.Scan(elementScanCriteria);

                while (elementEnumerator.MoveNext())
                {
                    yield return (Element)elementEnumerator.Current;
                }
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
        public IOwnerContainerMicrostation ToOwnerContainerMicrostation()
        {
            return new OwnerContainerMicrostation(this);
        }

    }
}
