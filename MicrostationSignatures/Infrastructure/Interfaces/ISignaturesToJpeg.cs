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
        void CreateJpegSignatures(string filePath);
    }
}