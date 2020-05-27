using System;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Обязательные поля штампа
    /// </summary>
    public class StampBasicFields: IStampBasicFields
    {
        public StampBasicFields(IStampTextField code, IStampTextField currentSheetNumber)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            CurrentSheetNumber = currentSheetNumber ?? throw new ArgumentNullException(nameof(currentSheetNumber));
        }

        /// <summary>
        /// Шифр
        /// </summary>
        public IStampTextField Code { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        public IStampTextField CurrentSheetNumber { get; }
    }
}