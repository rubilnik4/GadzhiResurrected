using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections
{
    /// <summary>
    /// Базовый текстовый элемент штампа Microstation
    /// </summary>  
    public interface IStampTextFieldMicrostation: IStampFieldMicrostation<ITextElementMicrostation>
    {
      
    }
}
