namespace GadzhiMicrostation.Microstation.Implementations.Units
{
    public static class UnitsNumeratorDenominator
    {
        /// <summary>
        /// Преобразовать численные коэффициенты масштабирования в тип измерения координат
        /// </summary>       
        public static UnitTypes GetUnitTypes(double UnitsNumerator, double UnitsDenominator)
        {
            UnitTypes unitTypes = UnitTypes.Millimeters;

            if (UnitsDenominator == 1.0d && UnitsNumerator == 1.0d)
            {
                unitTypes = UnitTypes.Meter;
            }
            else if (UnitsDenominator == 0.001d && UnitsNumerator == 1d)
            {
                unitTypes = UnitTypes.Millimeters;
            }
            else if (UnitsDenominator == 1.0d && UnitsNumerator == 1000000.0d)
            {
                unitTypes = UnitTypes.Micrometers;
            }
            else if (UnitsDenominator == 1.0d && UnitsNumerator == 1000.0d)
            {
                unitTypes = UnitTypes.Millimeters;
            }
            else if (UnitsDenominator == 1.0d && UnitsNumerator == 100.0d)
            {
                unitTypes = UnitTypes.Centimeters;
            }
            else if (UnitsDenominator == 1.0d && UnitsNumerator == 10.0d)
            {
                unitTypes = UnitTypes.Decimeters;
            }
            else if (UnitsDenominator == 1000.0d && UnitsNumerator == 1.0d)
            {
                unitTypes = UnitTypes.Kilometers;
            }
            else if (UnitsDenominator == 254.0d && UnitsNumerator == 10000000000.0d)
            {
                unitTypes = UnitTypes.MicroInches;
            }
            else if (UnitsDenominator == 254.0d && UnitsNumerator == 10000000.0d)
            {
                unitTypes = UnitTypes.Mils;
            }
            else if (UnitsDenominator == 1000.0 && UnitsNumerator == 39370.0d)
            {
                unitTypes = UnitTypes.SurveyInches;
            }
            else if (UnitsDenominator == 254.0 && UnitsNumerator == 10000.0d)
            {
                unitTypes = UnitTypes.Inches;
            }
            else if (UnitsDenominator == 254.0d * 12.0d && UnitsNumerator == 10000.0d)
            {
                unitTypes = UnitTypes.Feet;
            }
            else if (UnitsDenominator == 254.0d * 12.0d && UnitsNumerator == 1000000.0d)
            {
                unitTypes = UnitTypes.dFeet;
            }
            else if (UnitsDenominator == 12000.0d && UnitsNumerator == 39370.0d)
            {
                unitTypes = UnitTypes.SurveyFeet;
            }
            else if (UnitsDenominator == 254.0d * 3.0d * 12.0d && UnitsNumerator == 10000.0d)
            {
                unitTypes = UnitTypes.Yard;
            }
            else if (UnitsDenominator == 254.0d * 5280.0d * 12.0d && UnitsNumerator == 10000.0d)
            {
                unitTypes = UnitTypes.Mile;
            }

            return unitTypes;
        }

    }
}
