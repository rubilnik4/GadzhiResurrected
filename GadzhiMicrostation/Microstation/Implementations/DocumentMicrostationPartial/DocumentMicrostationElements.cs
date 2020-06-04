using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampContainer;
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
        /// Контейнер штампов
        /// </summary>
        private IStampContainer _stampContainer;

        /// <summary>
        /// Контейнер штампов
        /// </summary>
        public IStampContainer GetStampContainer(ConvertingSettingsApplication convertingSettings) =>
            _stampContainer ??= new StampContainer(FindStamps(ModelsMicrostation, convertingSettings), StampContainerType.United);

        /// <summary>
        /// Найти штампы во всех моделях и листах
        /// </summary>
        private static IResultAppCollection<IStamp> FindStamps(IEnumerable<IModelMicrostation> modelsMicrostation,
                                                               ConvertingSettingsApplication convertingSettings) =>
            modelsMicrostation.SelectMany((model, modelIndex) => model.FindStamps(modelIndex, convertingSettings)).
            Map(stamps => new ResultAppCollection<IStamp>(new ErrorApplication(ErrorApplicationType.StampNotFound, "Штампы не найдены")));
    }
}
