using GadzhiWord.Extension;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Enums;
using GadzhiWord.Models.StampCollections;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.StampPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public class StampWord : IStampWord
    {
        /// <summary>
        /// Элемент таблица опредеяющая штамп
        /// </summary>
        private readonly Table _tableStamp;

     //   private readonly IDocumentWord _documentWord;

        public StampWord(Table tableStamp)
        {
            _tableStamp = tableStamp;
            //_documentWord = documentWord;

            var stampType = FillFields();
            if (stampType.HasValue)
            {
                StampType = stampType.Value;
            }
            else
            {
                throw new ArgumentException(nameof(StampType));
            }

            StampContainer = new StampContainer();
        }

        /// <summary>
        /// Контейнер штампа, содержащий составные части
        /// </summary>
        public StampContainer StampContainer { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public StampType StampType { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name => $"{StampAdditionalParameters.StampTypeToString[StampType]}. Лист {_tableStamp.Range.GetPageNumber()}";

        /// <summary>
        /// Заполнить поля штампа
        /// </summary>
        private StampType? FillFields()
        {
            StampType? stampType = null;

            foreach (Cell cell in _tableStamp.Range.Cells)
            {
                if (cell != null && !String.IsNullOrWhiteSpace(cell.Range.Text))
                {
                    string cellText = StringAdditionalExtensions.PrepareCellTextToComprare(cell.Range.Text);

                    stampType = CheckStampType(stampType, cellText);

                    switch (CheckFieldType.GetStampFieldType(cellText))
                    {
                        case StampFieldType.PersonSignature:
                            
                            break;
                    }

                }
            }

            return stampType;
        }

        /// <summary>
        /// Определить тип штампа
        /// </summary>       
        private StampType? CheckStampType(StampType? stampType, string cellText)
        {
            if (stampType != StampType.Main)
            {
                if (stampType != StampType.Additional &&
                    StampAdditionalParameters.MarkersAdditionalStamp.MarkerContain(cellText))
                {
                    stampType = StampType.Additional;
                }
                if (StampAdditionalParameters.MarkersMainStamp.MarkerContain(cellText))
                {
                    stampType = StampType.Main;
                }
            }
            return stampType;
        }
    }
}
