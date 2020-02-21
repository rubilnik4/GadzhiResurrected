using System;

namespace GadzhiMicrostation.Microstation.Implementations.Units
{
    /// <summary>
    /// Тип измерения координат
    /// </summary>
    [Flags]
    public enum UnitTypes
    {
        Kilometers,
        Meter,
        Decimeters,
        Centimeters,
        Millimeters,
        Micrometers,
        Mils,
        Mile,
        Yard,
        SurveyFeet,
        Feet,
        dFeet,
        SurveyInches,
        Inches,
        MicroInches
    }
}
