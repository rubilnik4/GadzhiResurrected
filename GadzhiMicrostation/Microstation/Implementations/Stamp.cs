using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Implementations.StampFeatures;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampFeatures;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using Microsoft.Practices.Unity;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Штамп
    /// </summary>
    public class Stamp : ElementMicrostation, IStamp
    {
        /// <summary>
        /// Экземпляр ячейки Microstation определяющей штамп
        /// </summary>
        private readonly CellElement _stampCellElement;

        /// <summary>
        /// Родительский элемент
        /// </summary>
        private readonly IModelMicrostation _ownerModelMicrostation;

        /// <summary>
        /// Доступные поля в Штампе
        /// </summary>
        private IDictionary<string, IElementMicrostation> _stampFields;

        /// <summary>
        /// Класс для работы с подписями
        /// </summary>      
        private readonly ISignaturesStamp _signaturesStamp;

        public Stamp(CellElement stampCellElement,
                     IModelMicrostation ownerModelMicrostation)
            : base((Element)stampCellElement, ownerModelMicrostation)
        {
            _stampCellElement = stampCellElement;
            _ownerModelMicrostation = ownerModelMicrostation;

            _stampFields = new Dictionary<string, IElementMicrostation>();

            _signaturesStamp = new SignaturesStamp(this);

            FillDataFields();
        }

        /// <summary>
        /// Масштаб штампа
        /// </summary>
        private double Scale => _stampCellElement.Scale.X;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно коэффициента сжатия штампа
        /// </summary>
        public override double UnitScale => Scale * _ownerModelMicrostation.UnitsMicrostation.Global;

        /// <summary>
        /// Найти элементы в словаре штампа по ключам
        /// </summary>
        public IEnumerable<IElementMicrostation> FindElementsInStampFields(IEnumerable<string> fieldSearch,
                                                                           ElementMicrostationType? elementMicrostationType = ElementMicrostationType.Element) =>
                                                 fieldSearch?.Where(_stampFields.ContainsKey).
                                                 Select(fieldName => _stampFields[fieldName]).
                                                 Where(fieldName => elementMicrostationType == ElementMicrostationType.Element ||
                                                                    fieldName.ElementType == elementMicrostationType);

        /// <summary>
        /// Найти элемент в словаре штампа по ключам
        /// </summary>
        public IElementMicrostation FindElementInStampFields(string fieldSearch)
        {
            if (_stampFields.ContainsKey(fieldSearch))
            {
                return _stampFields[fieldSearch];
            }

            return null;
        }

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public void CompressFieldsRanges()
        {
            foreach (var element in _stampFields.Values)
            {
                switch (element.ElementType)
                {
                    case ElementMicrostationType.TextElement:
                        element.AsTextElementMicrostation.CompressRange();
                        break;
                    case ElementMicrostationType.TextNodeElement:
                        if (element.AsTextNodeElementMicrostation.CompressRange())
                        {
                            FindAndChangeSubElement(element.Id);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertSignatures()
        {
            _signaturesStamp.InsertSignatures();
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteSignatures()
        {
            _signaturesStamp.DeleteSignatures();
        }

        /// <summary>
        /// Заполнить поля данных
        /// </summary>
        private void FillDataFields()
        {
            if (_stampCellElement != null)
            {
                ElementEnumerator elementEnumerator = _stampCellElement.GetSubElements();

                while (elementEnumerator.MoveNext())
                {
                    var element = (Element)elementEnumerator.Current;
                    if (element.IsTextElement || element.IsTextNodeElement)
                    {
                        string controlName = element.GetAttributeControlName();

                        if (StampElement.ContainControlName(controlName))
                        {
                            AddElementToDictionary(controlName, element);
                        }
                        else
                        {
                            FillFormat(element);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Найти и заполнить поле формат
        /// </summary>
        private void FillFormat(Element element)
        {
            if (element.IsTextElement)
            {
                string textOfElement = element.AsTextElement.Text;
                if (StampMain.IsFormatField(textOfElement))
                {
                    AddElementToDictionary(StampMain.Format.Name, element);
                }
            }
        }

        /// <summary>
        /// Добавить элемент в словарь
        /// </summary>       
        private void AddElementToDictionary(string controlName, Element element)
        {
            StampBaseField stampBaseField = StampElement.GetBaseParametersByControlName(controlName);

            if (element.IsTextElement)
            {
                _stampFields.Add(controlName,
                                 new TextElementMicrostation(element.AsTextElement,
                                                             this,
                                                             stampBaseField.IsNeedCompress,
                                                             stampBaseField.IsVertical));
            }
            else if (element.IsTextNodeElement)
            {
                _stampFields.Add(controlName,
                                 new TextNodeElementMicrostation(element.AsTextNodeElement,
                                                                 this,
                                                                 stampBaseField.IsNeedCompress,
                                                                 stampBaseField.IsVertical));
            }

        }

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент. Только для внешних операций типа Scale, Move
        /// </summary>
        private void FindAndChangeSubElement(long Id)
        {
            while (_stampCellElement.MoveToNextElement(true))
            {
                var elementCurrent = _stampCellElement.CopyCurrentElement();
                if (elementCurrent.ID64 == Id)
                {
                    _stampCellElement.ReplaceCurrentElement(elementCurrent);
                    break;
                }
            }
        }
    }
}
