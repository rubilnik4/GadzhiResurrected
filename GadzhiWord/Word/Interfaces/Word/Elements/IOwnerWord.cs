namespace GadzhiWord.Word.Interfaces.Word.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public interface IOwnerWord
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        IApplicationOffice ApplicationOffice { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        IDocumentWord DocumentWord { get; }
    }
}
