namespace GadzhiMicrostationSignatures.Models.Interfaces
{
    /// <summary>
    /// Параметры и установки
    /// </summary>
    public interface IProjectSignatureSettings
    {
        /// <summary>
        /// Подписи для Microstation
        /// </summary>
        string SignatureMicrostationFileName { get; }

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        string StampMicrostationFileName { get; }

        /// <summary>
        /// Путь к файлу шаблону для преобразования подписей
        /// </summary>
        string SignatureTemplateFilePath { get; }

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        string DataResourcesFolder { get; }
    }
}