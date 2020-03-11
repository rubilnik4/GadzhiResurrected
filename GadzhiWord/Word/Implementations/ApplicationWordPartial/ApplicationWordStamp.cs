using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiConverting.Word.Interfaces.Elements;
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
        public IStampContainer StampWord => new StampContainer(FindTableStamps(), ActiveDocument);

        /// <summary>
        /// Найти штампы в документе
        /// </summary>    
        private IEnumerable<ITableElement> FindTableStamps() => ActiveDocument?.GetTablesInFooters().Where(CheckFooterIsStamp);                                                               

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(ITableElement tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => StringAdditionalExtensions.PrepareCellTextToCompare(cell?.Text)).
                                                                       Any(cellText => StampAdditionalParameters.MarkersMainStamp.MarkerContain(cellText));

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public void InsertStampSignatures()
        {
            var personSignatures = StampWord?.GetStampPersonSignatures();
            foreach (var personSignature in personSignatures)
            {
                personSignature.Signature.CellElementWord.DeleteAllPictures();
                personSignature.Signature.CellElementWord.InsertPicture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "WordData\\", "signature.jpg"));
            }
        }

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public void DeleteStampSignatures()
        {
            var personSignatures = StampWord?.GetStampPersonSignatures();
            foreach (var personSignature in personSignatures)
            {
                personSignature.Signature.CellElementWord.DeleteAllPictures();
            }
        }
    }
}
