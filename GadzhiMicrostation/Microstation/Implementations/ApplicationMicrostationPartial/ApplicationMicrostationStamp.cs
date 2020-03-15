using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Подкласс для обработки штампа
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationLibraryStamp
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer StampContainer => new StampContainer(ActiveDocument?.FindStamps());
     
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertStampSignatures()
        {
         
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteStampSignatures()
        {
          
        }
    }
}
