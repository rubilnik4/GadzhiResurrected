using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public class OwnerContainerMicrostation : IOwnerContainerMicrostation
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IModelMicrostation ModelMicrostation { get; }

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public double UnitScale { get; }

        public OwnerContainerMicrostation(IModelMicrostation modelMicrostation)
        {
            ApplicationMicrostation = modelMicrostation.ApplicationMicrostation;
            ModelMicrostation = modelMicrostation;
            UnitScale = modelMicrostation.UnitScale;
        }
    }
}
