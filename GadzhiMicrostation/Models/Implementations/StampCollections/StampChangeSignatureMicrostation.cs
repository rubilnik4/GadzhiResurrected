using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
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
                                                string personId, string personName,
                                                Func<string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : base(insertSignatureFunc)
        {
            if (String.IsNullOrEmpty(personId)) throw new ArgumentNullException(nameof(personId));

            PersonId = personId;                        
            PersonName = personName;
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
        public ITextElementMicrostation TypeOfChangeElement => TypeOfChange.ElementStamp.AsTextElementMicrostation;

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
        /// Идентефикатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName { get; }
    }
}
