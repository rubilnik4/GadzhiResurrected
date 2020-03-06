using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Штамп
    /// </summary>
    public class StampWord : IStampWord
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        public IEnumerable<IStamp> Stamps { get; }

        public StampWord(IEnumerable<ITableElementWord> tableStamps)
        {
            Stamps = InitializeStamp(tableStamps);
        }

        /// <summary>
        /// Корретность загрузки штампов
        /// </summary>
        public bool IsValid => Stamps != null;

        /// <summary>
        /// Получить список всех строк с подписями во всех штампах
        /// </summary>     
        public IEnumerable<IStampPersonSignature> GetStampPersonSignatures() => Stamps?.OfType<IStampMain>()?.
                                                                                SelectMany(stamp => stamp.StampPersonSignatures);

        /// <summary>
        /// Инициализировать штамп
        /// </summary>       
        private IEnumerable<IStampMain> InitializeStamp(IEnumerable<ITableElementWord> tableStamps) =>
                tableStamps?.Where(table => GetStampType(table) == StampType.Main).
                             Select(table => new StampMain(table));

        /// <summary>
        /// Заполнить поля штампа
        /// </summary>
        private StampType? GetStampType(ITableElementWord tableStamp)
        {
            StampType? stampType = null;

            foreach (var cell in tableStamp.CellsElementWord)
            {
                if (cell != null && !String.IsNullOrWhiteSpace(cell.Text))
                {
                    string cellText = StringAdditionalExtensions.PrepareCellTextToComprare(cell.Text);
                    stampType = CheckStampType(stampType, cellText);
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
