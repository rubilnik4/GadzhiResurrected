using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Строка с изменениями
    /// </summary>
    public interface IStampChange<out TField> : IStampSignature<TField>
        where TField : IStampField
    {
        /// <summary>
        /// Номер изменения
        /// </summary>
        TField NumberChange { get; }

        /// <summary>
        /// Количество участков
        /// </summary>
        TField NumberOfPlots { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        TField TypeOfChange { get; }

        /// <summary>
        /// Номер документа
        /// </summary>
        TField DocumentChange { get; }       

        /// <summary>
        /// Дата изменения
        /// </summary>
        TField DateChange { get; }
    }
}
