using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    public interface IApplicationMicrostationCommands
    {
        /// <summary>
        /// Создать ячейку на освнове шаблона в библиотеке
        /// </summary>       
        IResultAppValue<ICellElementMicrostation> CreateCellElementFromLibrary(string cellName, PointMicrostation origin,
                                                              IModelMicrostation modelMicrostation,
                                                              Func<ICellElementMicrostation, ICellElementMicrostation> additionalParametrs = null,
                                                              string cellDescription = null);

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        IResultAppValue<ICellElementMicrostation> CreateSignatureFromLibrary(string cellName, PointMicrostation origin,
                                                            IModelMicrostation modelMicrostation,
                                                            Func<ICellElementMicrostation, ICellElementMicrostation> additionalParametrs = null,
                                                            string cellDescription = null);

        /// <summary>
        /// Подключить библиотеку
        /// </summary>      
        void AttachLibrary(string libraryPath);

        /// <summary>
        /// Отключить библиотеку
        /// </summary>      
        void DetachLibrary();
       
    }
}
