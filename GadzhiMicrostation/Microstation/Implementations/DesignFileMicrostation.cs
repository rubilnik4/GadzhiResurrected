using GadzhiMicrostation.Microstation.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Текущий файл Microstation
    /// </summary>
    public class DesignFileMicrostation : IDesignFileMicrostation
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly DesignFile _designFileMicrostation;

        public DesignFileMicrostation(DesignFile designFileMicrostation)
        {
            _designFileMicrostation = designFileMicrostation;
        }

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDesingFileValid => _designFileMicrostation != null;

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        public IList<IModelMicrostation> ModelsMicrostation
        {
            get
            {
                List<IModelMicrostation> modelsMicrostation = new List<IModelMicrostation>();
                foreach (ModelReference model in _designFileMicrostation.Models)
                {
                    modelsMicrostation.Add(new ModelMicrostation(model));
                }
                return modelsMicrostation;
            }
        }

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IList<IStamp> Stamps => ModelsMicrostation.SelectMany(model => model.FindStamps()).
                                                          ToList();

    }
}
