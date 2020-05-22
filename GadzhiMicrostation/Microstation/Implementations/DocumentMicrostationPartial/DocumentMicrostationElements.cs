using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces;

namespace GadzhiMicrostation.Microstation.Implementations.DocumentMicrostationPartial
{
    /// <summary>
    /// Файл файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentLibraryElements
    {
        /// <summary>
        /// Загруженные штампы
        /// </summary>
        private IStampContainer _stampContainer;

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer GetStampContainer(ConvertingSettingsApplication convertingSettings) =>
            _stampContainer ??= new StampContainer(FindStamps(ModelsMicrostation, convertingSettings), FullName);

        /// <summary>
        /// Найти таблицы-штампы во всех моделях и листах
        /// </summary>
        private static IEnumerable<IStamp> FindStamps(IEnumerable<IModelMicrostation> modelsMicrostation, ConvertingSettingsApplication convertingSettings) =>
            modelsMicrostation.SelectMany((model, modelIndex) => model.FindStamps(modelIndex, convertingSettings)).ToList();
    }
}
