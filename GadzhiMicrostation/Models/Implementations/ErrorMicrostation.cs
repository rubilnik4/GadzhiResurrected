using GadzhiMicrostation.Models.Enum;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Ошибка  Microstation
    /// </summary>
    public class ErrorMicrostation
    {
        public ErrorMicrostation(ErrorMicrostationType errorMicrostationType,
                                 string errorDescription)
        {
            ErrorMicrostationType = errorMicrostationType;
            ErrorDescription = errorDescription;
        }

        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        public ErrorMicrostationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string ErrorDescription { get; }
    }
}
