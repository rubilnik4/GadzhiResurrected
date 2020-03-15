using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Штамп. Базовый класс
    /// </summary>
    public abstract class Stamp : IStamp
    {
        public Stamp()
        {
          
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public abstract StampType StampType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public abstract string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public abstract OrientationType Orientation { get; }

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public abstract void InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public abstract void DeleteSignatures();
    }
}
