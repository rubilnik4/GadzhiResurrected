namespace MicrostationSignatures.Models.Interfaces
{
    /// <summary>
    /// Параметры и установки
    /// </summary>
    public interface IProjectSignatureSettings
    {
        /// <summary>
        /// Путь к файлу шаблону для преобразования подписей
        /// </summary>
        public string SignatureTemplateFilePath { get; }
    }
}