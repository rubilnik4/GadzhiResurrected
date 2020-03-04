using GadzhiWord.Extension;
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

        private readonly IDocumentWord _documentWord;

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
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        private StampType StampType { get; }

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
                    string cellText = StringExtensions.PrepareCellTextToComprare(cell.Range.Text);
                    if (stampType != StampType.Main)
                    {
                        if (stampType != StampType.Additional &&
                            StampAdditionalParameters.MarkersAdditionalStamp.Any(marker => cellText.Contains(marker)))
                        {
                            stampType = StampType.Additional;
                        }
                        if (StampAdditionalParameters.MarkersMainStamp.Any(marker => cellText.Contains(marker)))
                        {
                            stampType = StampType.Main;
                        }
                    }


                }
            }

            return stampType;
        }
    }
}
