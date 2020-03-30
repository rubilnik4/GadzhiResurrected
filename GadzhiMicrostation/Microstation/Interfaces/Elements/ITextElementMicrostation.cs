namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public interface ITextElementMicrostation : IRangeBaseElementMicrostation<ITextElementMicrostation>
    {
        /// <summary>
        /// Текст элемента
        /// </summary>
        string Text { get; }
    }
}
