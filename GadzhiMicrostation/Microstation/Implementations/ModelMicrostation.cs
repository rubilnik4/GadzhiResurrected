using GadzhiMicrostation.Microstation.Implementations.StampPartial;
using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Найти штампы в модели
        /// </summary>    
        public IEnumerable<IStamp> FindStamps()
        {
            if (_modelMicrostation != null)
            {
                var elementScanCriteria = new ElementScanCriteria();

                elementScanCriteria.ExcludeAllTypes();
                elementScanCriteria.IncludeType(MsdElementType.msdElementTypeCellHeader);

                ElementEnumerator elementEnumerator = _modelMicrostation.Scan(elementScanCriteria);

                while (elementEnumerator.MoveNext())
                {
                    CellElement cellElement = (CellElement)elementEnumerator.Current;
                    string cellElementName = cellElement.Name.ToUpper();

                    if (cellElementName.StartsWith("STAMP") &&
                        !cellElementName.Contains("STAMP_AUDIT") &&
                        !cellElementName.Contains("STAMP_ISM"))
                    {
                        yield return new Stamp(cellElement, this);
                    }
                }
            }
        }
    }
}
