using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces;

namespace GadzhiMicrostation.Microstation.Implementations.DocumentMicrostationPartial
{
    /// <summary>
    /// Файл файл Microstation
    /// </summary>
    public partial class DocumentMicrostation
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        private IResultAppCollection<IStamp> _stamps;

        /// <summary>
        /// Список штампов
        /// </summary>
        public IResultAppCollection<IStamp> GetStamps(ConvertingSettingsApplication convertingSettings) =>
            _stamps ??= FindStamps(ModelsMicrostation, convertingSettings);

        /// <summary>
        /// Найти штампы во всех моделях и листах
        /// </summary>
        private static IResultAppCollection<IStamp> FindStamps(IEnumerable<IModelMicrostation> modelsMicrostation, 
                                                               ConvertingSettingsApplication convertingSettings) =>
            modelsMicrostation.SelectMany((model, modelIndex) => model.FindStamps(modelIndex, convertingSettings)).
            Map(stamps => new ResultAppCollection<IStamp>(new ErrorApplication(ErrorApplicationType.StampNotFound, "Штампы не найдены")));
    }
}
