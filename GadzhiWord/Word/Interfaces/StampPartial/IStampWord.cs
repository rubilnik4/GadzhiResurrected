using GadzhiWord.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public interface IStampWord
    {
        /// <summary>
        /// Тип штампа
        /// </summary>
        StampType StampType { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }
    }
}
