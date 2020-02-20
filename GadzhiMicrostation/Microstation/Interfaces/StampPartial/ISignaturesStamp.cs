using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.StampPartial
{
    /// <summary>
    /// Класс для работы с подписями
    /// </summary>
    public interface ISignaturesStamp
    {

        /// <summary>
        /// Вставить подписи
        /// </summary>
        void InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        void DeleteSignatures();
    }
}
