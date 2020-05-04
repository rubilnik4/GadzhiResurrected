using static GadzhiMicrostation.Models.Implementations.Coordinates.PointMicrostation;

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
            var masterUnitsType = UnitsNumeratorDenominator.GetUnitTypes(masterUnitsPerBaseNumerator,
                                                                               masterUerBaseDenominator);

            var subUnitsType = UnitsNumeratorDenominator.GetUnitTypes(subUnitsPerBaseNumerator,
                                                                      subUnitsPerBaseDenominator);

            double global = 1;

            if (masterUnitsType == UnitTypes.Inches &&
                subUnitsType == UnitTypes.Mils &&
                CompareCoordinate(masterUnitsPerBaseNumerator, 10000d) &&
                CompareCoordinate(masterUerBaseDenominator, 254d) &&
                CompareCoordinate(subUnitsPerBaseNumerator, 10000000d) &&
                CompareCoordinate(subUnitsPerBaseDenominator, 254d) &&
                CompareCoordinate(uorsPerStorageUnit, 2.54d))
            {
                global = 50 / 1.25;
            }

            else if (masterUnitsType == UnitTypes.Feet &&
                     subUnitsType == UnitTypes.DFeet &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 10000d) &&
                     CompareCoordinate(masterUerBaseDenominator, 3048d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 3048d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 5 / 1.5235457063711912;
            }

            else if (masterUnitsType == UnitTypes.Centimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 100;
            }

            else if (masterUnitsType == UnitTypes.Centimeters &&
                     subUnitsType == UnitTypes.Centimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 10000d))
            {
                global = 100;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Meter &&
                  CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                  CompareCoordinate(masterUerBaseDenominator, 1d) &&
                  CompareCoordinate(subUnitsPerBaseNumerator, 1d) &&
                  CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                  CompareCoordinate(uorsPerStorageUnit, 10d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                 subUnitsType == UnitTypes.Meter &&
                 CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                 CompareCoordinate(masterUerBaseDenominator, 1d) &&
                 CompareCoordinate(subUnitsPerBaseNumerator, 1d) &&
                 CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                 CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                subUnitsType == UnitTypes.Meter &&
                CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                CompareCoordinate(masterUerBaseDenominator, 1d) &&
                CompareCoordinate(subUnitsPerBaseNumerator, 1d) &&
                CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                CompareCoordinate(uorsPerStorageUnit, 1000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 10000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                    subUnitsType == UnitTypes.Centimeters &&
                    CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                    CompareCoordinate(masterUerBaseDenominator, 1d) &&
                    CompareCoordinate(subUnitsPerBaseNumerator, 100d) &&
                    CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                    CompareCoordinate(uorsPerStorageUnit, 1d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 100d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 1000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Centimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 10d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 10000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                    subUnitsType == UnitTypes.Decimeters &&
                    CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                    CompareCoordinate(masterUerBaseDenominator, 1d) &&
                    CompareCoordinate(subUnitsPerBaseNumerator, 10d) &&
                    CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                    CompareCoordinate(uorsPerStorageUnit, 10000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Millimeters &&
                  CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                  CompareCoordinate(masterUerBaseDenominator, 1d) &&
                  CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                  CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                  CompareCoordinate(uorsPerStorageUnit, 10000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                  subUnitsType == UnitTypes.Millimeters &&
                  CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                  CompareCoordinate(masterUerBaseDenominator, 1d) &&
                  CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                  CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                  CompareCoordinate(uorsPerStorageUnit, 1000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                 subUnitsType == UnitTypes.Millimeters &&
                 CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                 CompareCoordinate(masterUerBaseDenominator, 1d) &&
                 CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                 CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                 CompareCoordinate(uorsPerStorageUnit, 100000d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Meter &&
                     subUnitsType == UnitTypes.Micrometers &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 0.1d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1d) &&
                     CompareCoordinate(masterUerBaseDenominator, 0.001d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 1000d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                     subUnitsType == UnitTypes.Millimeters &&
                     CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(masterUerBaseDenominator, 1d) &&
                     CompareCoordinate(subUnitsPerBaseNumerator, 1000d) &&
                     CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                     CompareCoordinate(uorsPerStorageUnit, 100000d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                   subUnitsType == UnitTypes.Micrometers &&
                   CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                   CompareCoordinate(masterUerBaseDenominator, 1d) &&
                   CompareCoordinate(subUnitsPerBaseNumerator, 1000000d) &&
                   CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                   CompareCoordinate(uorsPerStorageUnit, 100d))
            {
                global = 1000;
            }

            else if (masterUnitsType == UnitTypes.Millimeters &&
                  subUnitsType == UnitTypes.Micrometers &&
                  CompareCoordinate(masterUnitsPerBaseNumerator, 1000d) &&
                  CompareCoordinate(masterUerBaseDenominator, 1d) &&
                  CompareCoordinate(subUnitsPerBaseNumerator, 1000000d) &&
                  CompareCoordinate(subUnitsPerBaseDenominator, 1d) &&
                  CompareCoordinate(uorsPerStorageUnit, 0.1d))
            {
                global = 1000;
            }

            Global = global;
        }
    }
}
