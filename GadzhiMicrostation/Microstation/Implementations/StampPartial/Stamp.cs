using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using Microsoft.Practices.Unity;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public partial class Stamp : CellElementMicrostation, IStamp
    {
        public Stamp(CellElement stampCellElement,
                     IModelMicrostation ownerModelMicrostation)
            : base(stampCellElement, ownerModelMicrostation)
        { 
            InitialStampDataFields();
        }

    }
}
