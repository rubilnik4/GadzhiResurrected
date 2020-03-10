using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationPartial
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public partial class ApplicationConverting : IApplicationConvertingPrinting
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer StampWord => new StampContainer(FindTableStamps(), this);

        /// <summary>
        /// Найти штампы в документе
        /// </summary>    
        private IEnumerable<ITableElement> FindTableStamps() => GetTablesInFooters().Where(CheckFooterIsStamp).
                                                                    Select(table => new TableElementWord(table));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(Table table) => table.Range.Cells.ToIEnumerable().
                                                              Where(cell => !String.IsNullOrWhiteSpace(cell?.Range?.Text)).
                                                              Select(cell => StringAdditionalExtensions.PrepareCellTextToCompare(cell.Range.Text)).
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
