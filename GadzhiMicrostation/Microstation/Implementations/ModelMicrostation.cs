using GadzhiMicrostation.Microstation.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public class ModelMicrostation: IModelMicrostation
    {
        /// <summary>
        /// Экземпляр модели или листа
        /// </summary>
        private readonly ModelReference _modelMicrostation;

        public ModelMicrostation(ModelReference modelMicrostation)
        {
            _modelMicrostation = modelMicrostation;
        }
    }
}
