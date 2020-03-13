using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Interfaces;
using System;
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
            var signatures = StampContainer?.GetStampPersonSignatures().
                                             Select(personSignature => personSignature.Signature).
                                             Cast<IStampFieldWord>();

            foreach (var signature in signatures)
            {
                signature.CellElementStamp.DeleteAllPictures();
                signature.CellElementStamp.InsertPicture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "WordData\\", "signature.jpg"));
            }
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteStampSignatures()
        {
            var signatures = StampContainer?.GetStampPersonSignatures().
                                            Select(personSignature => personSignature.Signature).
                                            Cast<IStampFieldWord>();

            foreach (var signature in signatures)
            {
                signature.CellElementStamp.DeleteAllPictures();
            }
        }
    }
}
