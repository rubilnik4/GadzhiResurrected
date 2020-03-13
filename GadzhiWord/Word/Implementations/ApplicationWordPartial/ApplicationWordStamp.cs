using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс для обработки штампа
    /// </summary>
    public partial class ApplicationWord : IApplicationLibraryStamp
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
            var personSignatures = StampContainer?.GetStampPersonSignatures();
            foreach (var personSignature in personSignatures)
            {
                personSignature.Signature.CellElementStamp.DeleteAllSignatures();
                personSignature.Signature.CellElementStamp.InsertSignature(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "WordData\\", "signature.jpg"));
            }
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteStampSignatures()
        {
            var personSignatures = StampContainer?.GetStampPersonSignatures();
            foreach (var personSignature in personSignatures)
            {
                personSignature.Signature.CellElementStamp.DeleteAllSignatures();
            }
        }
    }
}
