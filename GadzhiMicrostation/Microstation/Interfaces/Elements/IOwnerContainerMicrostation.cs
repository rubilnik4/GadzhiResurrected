using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public interface IOwnerContainerMicrostation
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        IModelMicrostation ModelMicrostation { get; }

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        double UnitScale { get; }
    }
}
