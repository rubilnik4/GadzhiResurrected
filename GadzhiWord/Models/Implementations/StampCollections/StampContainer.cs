using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Контейнер штампов
    /// </summary>
    public class StampContainer : IStampContainer
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        public IEnumerable<IStamp> Stamps { get; }

        public StampContainer(IEnumerable<IStamp> stamps)
        {
            Stamps = stamps;
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

        ///// <summary>
        ///// Инициализировать штамп
        ///// </summary>       
        //private IEnumerable<IStampMain> InitializeStamp(IEnumerable<ITableElement> tableStamps, IDocumentLibrary documentWord) =>
        //        tableStamps?.Where(table => GetStampType(table) == StampType.Main).
        //                     Select(table => new StampMain(table, documentWord));

        ///// <summary>
        ///// Заполнить поля штампа
        ///// </summary>
        //private StampType? GetStampType(ITableElement tableStamp)
        //{
        //    StampType? stampType = null;

        //    foreach (var cell in tableStamp.CellsElement)
        //    {
        //        if (cell != null && !String.IsNullOrWhiteSpace(cell.Text))
        //        {
        //            string cellText = StringAdditionalExtensions.PrepareCellTextToCompare(cell.Text);
        //            stampType = CheckStampType(stampType, cellText);
        //        }
        //    }

        //    return stampType;
        //}

        ///// <summary>
        ///// Определить тип штампа
        ///// </summary>       
        //private StampType? CheckStampType(StampType? stampType, string cellText)
        //{
        //    if (stampType != StampType.Main)
        //    {
        //        if (stampType != StampType.Additional &&
        //            StampAdditionalParameters.MarkersAdditionalStamp.MarkerContain(cellText))
        //        {
        //            stampType = StampType.Additional;
        //        }
        //        if (StampAdditionalParameters.MarkersMainStamp.MarkerContain(cellText))
        //        {
        //            stampType = StampType.Main;
        //        }
        //    }
        //    return stampType;
        //}
    }
}
