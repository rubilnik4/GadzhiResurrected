using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiMicrostation.Microstation.Implementations;
using System;
using GadzhiApplicationCommon.Models.Implementation.Resources;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public interface IApplicationMicrostation : IApplicationLibrary<IDocumentMicrostation>, IApplicationMicrostationCommands, 
                                                IApplicationMicrostationPrinting
    {
        /// <summary>
        /// Ресурсы, используемые модулем Microstation
        /// </summary>
        MicrostationResources MicrostationResources { get; }
    }
}
