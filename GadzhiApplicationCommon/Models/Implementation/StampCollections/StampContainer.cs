using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

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
            var errorNull = new ErrorApplication(ErrorApplicationType.StampNotFound, $"Штампы в файле {Path.GetFileName(filePath)} не найдены");
            Stamps = new ResultAppCollection<IStamp>(stamps, errorNull);
        }
    }
}
