using static GadzhiMicrostation.Models.Implementations.Coordinates.PointMicrostation;

namespace GadzhiMicrostation.Microstation.Implementations.Units
{
    public static class UnitsNumeratorDenominator
    {
        /// <summary>
        /// Преобразовать численные коэффициенты масштабирования в тип измерения координат
        /// </summary>       
        public static UnitTypes GetUnitTypes(double unitsNumerator, double unitsDenominator) =>
            unitsDenominator switch
            {
                _ when CompareCoordinate(unitsNumerator, 1.0d) && CompareCoordinate(unitsDenominator, 1.0d) => UnitTypes.Meter,
                _ when CompareCoordinate(unitsNumerator, 1d) && CompareCoordinate(unitsDenominator, 0.001d) => UnitTypes.Millimeters,
                _ when CompareCoordinate(unitsNumerator, 1000000.0d) && CompareCoordinate(unitsDenominator, 1.0d) => UnitTypes.Micrometers,
                _ when CompareCoordinate(unitsNumerator, 1000.0d) && CompareCoordinate(unitsDenominator, 1.0d) => UnitTypes.Millimeters,
                _ when CompareCoordinate(unitsNumerator, 100.0d) && CompareCoordinate(unitsDenominator, 1.0d) => UnitTypes.Centimeters,
                _ when CompareCoordinate(unitsNumerator, 10.0d) && CompareCoordinate(unitsDenominator, 1.0d) => UnitTypes.Decimeters,
                _ when CompareCoordinate(unitsNumerator, 1.0d) && CompareCoordinate(unitsDenominator, 1000.0d) => UnitTypes.Kilometers,
                _ when CompareCoordinate(unitsNumerator, 10000000000.0d) && CompareCoordinate(unitsDenominator, 254.0d) => UnitTypes.MicroInches,
                _ when CompareCoordinate(unitsNumerator, 10000000.0d) && CompareCoordinate(unitsDenominator, 254.0d) => UnitTypes.Mils,
                _ when CompareCoordinate(unitsNumerator, 39370.0d) && CompareCoordinate(unitsDenominator, 1000.0) => UnitTypes.SurveyInches,
                _ when CompareCoordinate(unitsNumerator, 10000.0d) && CompareCoordinate(unitsDenominator, 254.0) => UnitTypes.Inches,
                _ when CompareCoordinate(unitsNumerator, 10000.0d) && CompareCoordinate(unitsDenominator, 254.0d * 12.0d) => UnitTypes.Feet,
                _ when CompareCoordinate(unitsNumerator, 1000000.0d) && CompareCoordinate(unitsDenominator, 254.0d * 12.0d) => UnitTypes.DFeet,
                _ when CompareCoordinate(unitsNumerator, 39370.0d) && CompareCoordinate(unitsDenominator, 12000.0d) => UnitTypes.SurveyFeet,
                _ when CompareCoordinate(unitsNumerator, 10000.0d) && CompareCoordinate(unitsDenominator, 254.0d * 3.0d * 12.0d) => UnitTypes.Yard,
                _ when CompareCoordinate(unitsNumerator, 10000.0d) && CompareCoordinate(unitsDenominator, 254.0d * 5280.0d * 12.0d) => UnitTypes.Mile,
                _ => UnitTypes.Millimeters
            };

    }
}
