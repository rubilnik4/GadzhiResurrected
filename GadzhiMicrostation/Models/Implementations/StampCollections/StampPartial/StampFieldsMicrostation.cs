using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс для работа с внутренними полями штампа
    /// </summary>
    public abstract partial class StampMicrostation
    {


        ///// <summary>
        ///// Инициализировать данные
        ///// </summary>
        //private void InitializeStampFields()
        //{
        //    _stampFields = new Dictionary<string, ElementMicrostationPair>();

        //    FillDataFields();
        //}

        ///// <summary>
        ///// Найти элемент в словаре штампа по ключам
        ///// </summary>
        //private IElementMicrostation FindElementInStampFields(string fieldSearch)
        //{
        //    if (_stampFields.ContainsKey(fieldSearch))
        //    {
        //        return StampFieldsWrapper[fieldSearch];
        //    }

        //    return null;
        //}

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        protected IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<string> fieldsSearch,
                                                                              ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
                                                 StampCellElement.SubElements.
                                                 Where(fieldName => elementMicrostationType == ElementMicrostationType.Element ||
                                                                    fieldName.ElementType == elementMicrostationType).
                                                 Join(fieldsSearch,
                                                      subElement => subElement.AttributeControlName,
                                                      fieldSearch => fieldSearch,
                                                      (subElement, fieldSearch) => subElement);

        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        public IStampFieldMicrostation GetFieldFromElements(IEnumerable<ITextElementMicrostation> elementsMicrostation,
                                                            HashSet<StampFieldBase> stampFields, StampFieldType stampFieldType) =>
                elementsMicrostation?.Where(element => stampFields?.
                                                       Select(field => field.Name).
                                                       Contains(element.AttributeControlName) == true)?.
                                      Select(field => new StampFieldMicrostation(field, stampFieldType))?.
                                      FirstOrDefault();

        ///// <summary>
        ///// Вписать текстовые поля в рамки
        ///// </summary>
        //private void CompressFieldsRanges()
        //{
        //    foreach (var elementPair in _stampFields.Values)
        //    {
        //        switch (elementPair.ElementWrapper.ElementType)
        //        {
        //            case ElementMicrostationType.TextElement:
        //                elementPair.ElementWrapper.AsTextElementMicrostation.CompressRange();
        //                break;
        //            case ElementMicrostationType.TextNodeElement:
        //                if (elementPair.ElementWrapper.AsTextNodeElementMicrostation.CompressRange())
        //                {
        //                    FindAndChangeSubElement(elementPair.ElementOriginal);
        //                }
        //                break;
        //        }
        //    }
        //}


        ///// <summary>
        ///// Доступные поля в штампе в формате обертки
        ///// </summary>
        //private IDictionary<string, IElementMicrostation> StampFieldsWrapper =>
        //    _stampFields.ToDictionary(pair => pair.Key,
        //                              pair => pair.Value.ElementWrapper);


        ///// <summary>
        ///// Заполнить поля данных
        ///// </summary>
        //private void FillDataFields()
        //{
        //    var subTextElements = GetSubElements()?.Where(subElement => subElement.ElementWrapper.ElementType == ElementMicrostationType.TextElement ||
        //                                                                subElement.ElementWrapper.ElementType == ElementMicrostationType.TextNodeElement);
        //    foreach (var subElement in subTextElements)
        //    {
        //        if (StampFieldElement.ContainControlName(subElement.ElementWrapper.AttributeControlName))
        //        {
        //            AddElementToDictionary(subElement);
        //        }
        //        else
        //        {
        //            FillFormat(subElement);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Найти и заполнить поле формат
        ///// </summary>
        //private void FillFormat(ElementMicrostationPair elementPair)
        //{
        //    if (elementPair.ElementWrapper.IsTextElementMicrostation)
        //    {
        //        var textElement = elementPair.ElementWrapper.AsTextElementMicrostation;
        //        if (StampFieldMain.IsFormatField(textElement.Text))
        //        {
        //            textElement.AttributeControlName = StampFieldMain.PaperSize.Name;
        //            AddElementToDictionary(elementPair);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Добавить элемент в словарь
        ///// </summary>       
        //private void AddElementToDictionary(ElementMicrostationPair elementPair)
        //{
        //    StampFieldBase stampBaseField = StampFieldElement.GetBaseParametersByControlName(elementPair.ElementWrapper.AttributeControlName);

        //    if (elementPair.ElementWrapper.IsTextElementMicrostation)
        //    {
        //        var textElement = elementPair.ElementWrapper.AsTextElementMicrostation;
        //        textElement.IsNeedCompress = stampBaseField.IsNeedCompress;
        //        textElement.IsVertical = stampBaseField.IsVertical;

        //        _stampFields.Add(elementPair.ElementWrapper.AttributeControlName, elementPair);
        //    }
        //    else if (elementPair.ElementWrapper.IsTextNodeElementMicrostation)
        //    {
        //        var textnodeElement = elementPair.ElementWrapper.AsTextNodeElementMicrostation;
        //        textnodeElement.IsNeedCompress = stampBaseField.IsNeedCompress;
        //        textnodeElement.IsVertical = stampBaseField.IsVertical;

        //        _stampFields.Add(elementPair.ElementWrapper.AttributeControlName, elementPair);
        //    }

        //}
    }
}
