using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы со штампом
    /// </summary>
    public partial class DocumentWord : IDocumentWordStamp
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampWord StampWord => new StampWord(FindTableStamps());

        /// <summary>
        /// Найти штампы в документе
        /// </summary>    
        private IEnumerable<ITableElementWord> FindTableStamps() => GetTablesInFooters().Where(CheckFooterIsStamp).
                                                                    Select(table => new TableElementWord(table));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(Table table) => table.Range.Cells.ToIEnumerable().
                                                              Where(cell => !String.IsNullOrWhiteSpace(cell?.Range?.Text)).
                                                              Select(cell => StringAdditionalExtensions.PrepareCellTextToComprare(cell.Range.Text)).
                                                              Any(cellText => StampAdditionalParameters.MarkersMainStamp.MarkerContain(cellText));

        /// <summary>
        /// Вставить подписи
        /// </summary>
        private void InsertStampSignatures()
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
        private void DeleteStampSignatures()
        {
            var personSignatures = StampWord?.GetStampPersonSignatures();
            foreach (var personSignature in personSignatures)
            {
                personSignature.Signature.CellElementWord.DeleteAllPictures();              
            }
        }
    }
}
