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
            FullCode = code ?? throw new ArgumentNullException(nameof(code));
            CurrentSheet = currentSheetNumber ?? throw new ArgumentNullException(nameof(currentSheetNumber));
        }

        /// <summary>
        /// Шифр
        /// </summary>
        public IStampTextField FullCode { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        public IStampTextField CurrentSheet { get; }
    }
}