using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с изменениями Microstation
    /// </summary>
    public class StampChangeSignatureMicrostation : StampSignatureMicrostation,
                                                    IStampChangeSignatureMicrostation
    {
        public StampChangeSignatureMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                                IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                                IStampFieldMicrostation dateChange,
                                                string attributePersonId)
          : this(numberChange, numberOfPlots, typeOfChange, documentChange, null, dateChange, attributePersonId) { }

        public StampChangeSignatureMicrostation(IStampChangeSignature<IStampFieldMicrostation> changeSignature)
          : this(changeSignature?.NumberChange, changeSignature?.NumberOfPlots, changeSignature?.TypeOfChange,
                 changeSignature?.DocumentChange, changeSignature?.Signature, changeSignature?.DateChange,
                 changeSignature?.AttributePersonId)
        {
            if (changeSignature == null)
                throw new ArgumentNullException(nameof(changeSignature));
        }

        public StampChangeSignatureMicrostation(IStampFieldMicrostation numberChange, IStampFieldMicrostation numberOfPlots,
                                                IStampFieldMicrostation typeOfChange, IStampFieldMicrostation documentChange,
                                                IStampFieldMicrostation signature, IStampFieldMicrostation dateChange,
                                                string attributePersonId)
            : base(signature)
        {
            AttributeChangePersonId = !String.IsNullOrEmpty(attributePersonId) ?
                                      attributePersonId :
                                      throw new ArgumentNullException(nameof(attributePersonId));
            NumberChange = numberChange;
            NumberOfPlots = numberOfPlots;
            TypeOfChange = typeOfChange;
            DocumentChange = documentChange;
            DateChange = dateChange;
        }

        /// <summary>
        /// Номер изменения
        /// </summary>
        public IStampFieldMicrostation NumberChange { get; }

        /// <summary>
        /// Номер изменения. Элемент
        /// </summary>
        public ITextElementMicrostation NumberChangeElement => NumberChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Количество участков
        /// </summary>
        public IStampFieldMicrostation NumberOfPlots { get; }

        /// <summary>
        ///Количество участков. Элемент
        /// </summary>
        public ITextElementMicrostation NumberOfPlotsElement => NumberOfPlots.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Тип изменения
        /// </summary>
        public IStampFieldMicrostation TypeOfChange { get; }

        /// <summary>
        /// Тип изменения. Элемент
        /// </summary>
        public ITextElementMicrostation TypeOfChangeElement => Signature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Номер докумета
        /// </summary>
        public IStampFieldMicrostation DocumentChange { get; }

        /// <summary>
        /// Номер докумета. Элемент
        /// </summary>
        public ITextElementMicrostation DocumentChangeElement => DocumentChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Дата изменения
        /// </summary>
        public IStampFieldMicrostation DateChange { get; }

        /// <summary>
        ///Дата изменения. Элемент
        /// </summary>
        public ITextElementMicrostation DateChangeElement => DateChange.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Идентефикатор личности для изменения
        /// </summary>    
        public string AttributeChangePersonId { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string AttributePersonId => AttributeChangePersonId;
    }
}
