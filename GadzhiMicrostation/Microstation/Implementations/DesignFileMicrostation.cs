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
        public IEnumerable<IModelMicrostation> ModelsMicrostation
        {
            get
            {
                foreach (ModelReference model in _designFileMicrostation.Models)
                {
                    yield return new ModelMicrostation(model);
                }
            }
        }

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IEnumerable<IStamp> FindAllStamps() =>
               ModelsMicrostation.SelectMany(model => model.FindStamps());

    }
}
