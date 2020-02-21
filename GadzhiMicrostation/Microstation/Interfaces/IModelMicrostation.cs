using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enums;
using System.Collections.Generic;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public interface IModelMicrostation
    {
        /// <summary>
        /// Порядковый идентефикационный номер
        /// </summary>
        string IdName { get; }

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        double UnitScale { get; }

        /// <summary>
        /// Найти элементы в модели по типу
        /// </summary>       
        IEnumerable<IElementMicrostation> GetModelElementsMicrostation(ElementMicrostationType includeTypeMicrostation);

        /// <summary>
        /// Найти элементы в модели по типам
        /// </summary>       
        IEnumerable<IElementMicrostation> GetModelElementsMicrostation(IEnumerable<ElementMicrostationType> includeTypesMicrostation = null);

        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        IEnumerable<IStamp> FindStamps();

        /// <summary>
        /// Удалить элемент
        /// </summary>      
        void RemoveElement(long id);

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        IOwnerContainerMicrostation ToOwnerContainerMicrostation();
    }
}
