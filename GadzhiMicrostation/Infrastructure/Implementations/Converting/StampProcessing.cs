using GadzhiMicrostation.Microstation.Interfaces.StampPartial;

namespace GadzhiMicrostation.Infrastructure.Implementations.Converting
{
    /// <summary>
    /// Обработка Штампа
    /// </summary>
    public class StampProcessing
    {
        /// <summary>
        /// Обработка штампа
        /// </summary>       
        public static void ConvertingStamp(IStamp stamp)
        {
            stamp.CompressFieldsRanges();

            stamp.InsertSignatures();
        }
    }
}
