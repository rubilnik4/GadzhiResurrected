using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Контейнер штампов. Базовый класс
    /// </summary>
    public class StampContainer : IStampContainer
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        public IResultAppCollection<IStamp> Stamps { get; }

        public StampContainer(IEnumerable<IStamp> stamps, string filePath)
        {
            Stamps = new ResultAppCollection<IStamp>(stamps,
                                                     new ErrorApplication(ErrorApplicationType.StampNotFound, 
                                                                          $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
        }
    }
}
