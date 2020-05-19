using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using MicrostationSignatures.Models.Enums;

namespace MicrostationSignatures.Infrastructure.Interfaces
{
    /// <summary>
    /// Преобразование подписей Microstation в Jpeg
    /// </summary>
    public interface ISignaturesToJpeg
    {
        /// <summary>
        /// Создать подписи из прикрепленной библиотеки Microstation в формате Jpeg
        /// </summary>
        Task<IResultError> SendJpegSignaturesToDataBase(string filePath);

        /// <summary>
        /// Создать подписи Microstation в базу данных
        /// </summary>
        Task<IResultError> SendMicrostationDataToDatabase(string filePathMicrostation, MicrostationDataType microstationDataType);
    }
}