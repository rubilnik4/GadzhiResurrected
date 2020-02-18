using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Units
{
    /// <summary>
    /// Коэффициенты преобразования координат в текущие
    /// </summary>
    public class UnitsMicrostation
    {
        /// <summary>
        ///  Коэффициент масштабирования для всех элементов модели
        /// </summary>
        public double Global { get; private set; } //DopalGlobal в прошлом

        public UnitsMicrostation(double masterUnitsPerBaseNumerator,
                                 double masterUerBaseDenominator,
                                 double subUnitsPerBaseNumerator,
                                 double subUnitsPerBaseDenominator,
                                 double uorsPerStorageUnit)
        {
            FillUnits(masterUnitsPerBaseNumerator,
                      masterUerBaseDenominator,
                      subUnitsPerBaseNumerator,
                      subUnitsPerBaseDenominator,
                      uorsPerStorageUnit);
        }

        /// <summary>
        /// Получить значения коэффициентов
        /// </summary>
        private void FillUnits(double masterUnitsPerBaseNumerator,
                               double masterUerBaseDenominator,
                               double subUnitsPerBaseNumerator,
                               double subUnitsPerBaseDenominator,
                               double uorsPerStorageUnit)
        {
            UnitTypes masterUnitsType = UnitsNumeratorDenominator.GetUnitTypes(masterUnitsPerBaseNumerator,
                                                                               masterUerBaseDenominator);

            UnitTypes subUnitsType = UnitsNumeratorDenominator.GetUnitTypes(subUnitsPerBaseNumerator,
                                                                            subUnitsPerBaseDenominator);

            double global = 1;

            if (masterUnitsType == UnitTypes.Inches &&
                subUnitsType == UnitTypes.Mils &&
                masterUnitsPerBaseNumerator == 10000d &&
                masterUerBaseDenominator == 254d &&
                subUnitsPerBaseNumerator == 10000000d &&
                subUnitsPerBaseDenominator == 254d &&
                uorsPerStorageUnit == 2.54d)
            {
                global = 50 / 1.25;
            }

            else if (masterUnitsType == UnitTypes.Feet &&
                     subUnitsType == UnitTypes.dFeet &&
                     masterUnitsPerBaseNumerator == 10000d &&
                     masterUerBaseDenominator == 3048d &&
                     subUnitsPerBaseNumerator == 1000000d &&
                     subUnitsPerBaseDenominator == 3048d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 5 / 1.5235457063711912;
            }

            else if (masterUnitsType == UnitTypes.Centimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 100d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 100;
            }

            else if (masterUnitsType == UnitTypes.Centimeters &&
                     subUnitsType == UnitTypes.Centimeters &&
                     masterUnitsPerBaseNumerator == 100d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 100d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 10000d)
            {
                global = 100;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Meter &&
                  masterUnitsPerBaseNumerator == 1d &&
                  masterUerBaseDenominator == 1d &&
                  subUnitsPerBaseNumerator == 1d &&
                  subUnitsPerBaseDenominator == 1d &&
                  uorsPerStorageUnit == 10d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                 subUnitsType == UnitTypes.Meter &&
                 masterUnitsPerBaseNumerator == 1d &&
                 masterUerBaseDenominator == 1d &&
                 subUnitsPerBaseNumerator == 1d &&
                 subUnitsPerBaseDenominator == 1d &&
                 uorsPerStorageUnit == 100d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                subUnitsType == UnitTypes.Meter &&
                masterUnitsPerBaseNumerator == 1d &&
                masterUerBaseDenominator == 1d &&
                subUnitsPerBaseNumerator == 1d &&
                subUnitsPerBaseDenominator == 1d &&
                uorsPerStorageUnit == 1000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 100d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 100d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 10000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                    subUnitsType == UnitTypes.Centimeters &&
                    masterUnitsPerBaseNumerator == 1d &&
                    masterUerBaseDenominator == 1d &&
                    subUnitsPerBaseNumerator == 100d &&
                    subUnitsPerBaseDenominator == 1d &&
                    uorsPerStorageUnit == 1d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 100d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 1000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 10d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 10000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                    subUnitsType == UnitTypes.Decimeters &&
                    masterUnitsPerBaseNumerator == 1d &&
                    masterUerBaseDenominator == 1d &&
                    subUnitsPerBaseNumerator == 10d &&
                    subUnitsPerBaseDenominator == 1d &&
                    uorsPerStorageUnit == 10000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Millimeters &&
                  masterUnitsPerBaseNumerator == 1d &&
                  masterUerBaseDenominator == 1d &&
                  subUnitsPerBaseNumerator == 1000d &&
                  subUnitsPerBaseDenominator == 1d &&
                  uorsPerStorageUnit == 10000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Millimeters &&
                  masterUnitsPerBaseNumerator == 1d &&
                  masterUerBaseDenominator == 1d &&
                  subUnitsPerBaseNumerator == 1000d &&
                  subUnitsPerBaseDenominator == 1d &&
                  uorsPerStorageUnit == 1000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                 subUnitsType == UnitTypes.Millimeters &&
                 masterUnitsPerBaseNumerator == 1d &&
                 masterUerBaseDenominator == 1d &&
                 subUnitsPerBaseNumerator == 1000d &&
                 subUnitsPerBaseDenominator == 1d &&
                 uorsPerStorageUnit == 100000d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Micrometers &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1000d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1000d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 0.1d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1000d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1d &&
                     masterUerBaseDenominator == 0.001d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 1000d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     masterUnitsPerBaseNumerator == 1000d &&
                     masterUerBaseDenominator == 1d &&
                     subUnitsPerBaseNumerator == 1000d &&
                     subUnitsPerBaseDenominator == 1d &&
                     uorsPerStorageUnit == 100000d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                   subUnitsType == UnitTypes.Micrometers &&
                   masterUnitsPerBaseNumerator == 1000d &&
                   masterUerBaseDenominator == 1d &&
                   subUnitsPerBaseNumerator == 1000000d &&
                   subUnitsPerBaseDenominator == 1d &&
                   uorsPerStorageUnit == 100d)
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                  subUnitsType == UnitTypes.Micrometers &&
                  masterUnitsPerBaseNumerator == 1000d &&
                  masterUerBaseDenominator == 1d &&
                  subUnitsPerBaseNumerator == 1000000d &&
                  subUnitsPerBaseDenominator == 1d &&
                  uorsPerStorageUnit == 0.1d)
            {
                global = 1000;
            }

            Global = global;
        }
    }
}
