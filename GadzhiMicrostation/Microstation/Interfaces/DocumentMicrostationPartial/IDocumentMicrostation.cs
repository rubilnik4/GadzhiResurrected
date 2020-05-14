using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;

namespace GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial
{
    public interface IDocumentMicrostation : IDocumentLibrary
    {
        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        IList<IModelMicrostation> ModelsMicrostation { get; }
    }
}
