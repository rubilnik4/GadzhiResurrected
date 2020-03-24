using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public class OwnerMicrostation : IOwnerMicrostation
    {
        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IModelMicrostation ModelMicrostation { get; }

        public OwnerMicrostation(IModelMicrostation modelMicrostation)
        {
            ModelMicrostation = modelMicrostation ?? throw new ArgumentNullException(nameof(modelMicrostation));
        }

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation => ModelMicrostation.ApplicationMicrostation;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public double UnitScale => ModelMicrostation.UnitScale;
    }
}
