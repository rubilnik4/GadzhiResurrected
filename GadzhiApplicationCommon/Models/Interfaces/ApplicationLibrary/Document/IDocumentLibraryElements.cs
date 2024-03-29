﻿using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document
{
    /// <summary>
    /// Подкласс документа для работы с элементами
    /// </summary>
    public interface IDocumentLibraryElements
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        IStampContainer GetStampContainer(ConvertingSettingsApp convertingSettings);
    }
}
