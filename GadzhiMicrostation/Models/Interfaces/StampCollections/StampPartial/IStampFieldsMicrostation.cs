using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial
{
    /// <summary>
    /// Поля штампа Microstation
    /// </summary>
    public interface IStampFieldsMicrostation
    {
        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        IStampTextFieldMicrostation GetFieldFromElements(IEnumerable<ITextElementMicrostation> elementsMicrostation,
                                                         HashSet<StampFieldBase> stampFields, StampFieldType stampFieldType);

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        IResultAppCollection<TElement> FindElementsInStamp<TElement>(IEnumerable<string> fieldsSearch, IErrorApplication errorNull)
            where TElement : IElementMicrostation;
    }
}