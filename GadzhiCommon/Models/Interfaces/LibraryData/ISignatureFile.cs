using GadzhiCommon.Models.Implementations.LibraryData;

namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    public interface ISignatureFile: ISignatureFileBase<PersonInformation>
    {
        /// <summary>
        /// Вертикальное расположение изображения
        /// </summary>
        bool IsVerticalImage { get; }
    }
}