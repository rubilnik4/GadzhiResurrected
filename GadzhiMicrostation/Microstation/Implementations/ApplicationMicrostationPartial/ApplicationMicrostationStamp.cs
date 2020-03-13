﻿using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Подкласс для обработки штампа
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationLibraryStamp
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer StampContainer => new StampContainer(ActiveDocument?.FindStamps());
     
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertStampSignatures()
        {
            //var personSignatures = StampWord?.GetStampPersonSignatures();
            //foreach (var personSignature in personSignatures)
            //{
            //    personSignature.Signature.CellElementWord.DeleteAllSignatures();
            //    personSignature.Signature.CellElementWord.InsertSignature(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "WordData\\", "signature.jpg"));
            //}
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteStampSignatures()
        {
            //var personSignatures = StampWord?.GetStampPersonSignatures();
            //foreach (var personSignature in personSignatures)
            //{
            //    personSignature.Signature.CellElementWord.DeleteAllSignatures();
            //}
        }
    }
}
