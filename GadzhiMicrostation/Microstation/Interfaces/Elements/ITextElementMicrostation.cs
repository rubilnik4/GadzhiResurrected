namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public interface ITextElementMicrostation : IRangeBaseElementMicrostation
    {
        /// <summary>
        /// Текст элемента
        /// </summary>
        string Text { get; }
    }
}
