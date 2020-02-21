namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public interface ICellElementMicrostation : IRangeBaseElementMicrostation
    {
        /// <summary>
        /// Имя ячейки
        /// </summary>
        string Name { get; }
    }
}
