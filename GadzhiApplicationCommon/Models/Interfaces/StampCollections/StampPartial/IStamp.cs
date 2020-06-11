using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public interface IStamp
    {
        /// <summary>
        /// Параметры штампа
        /// </summary>
        StampSettings StampSettings { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        StampType StampType { get; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        StampApplicationType StampApplicationType { get; }

        /// <summary>
        /// Основные поля штампа
        /// </summary>
        IStampBasicFields StampBasicFields { get; }

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        IStampSignatureFields StampSignatureFields { get; }

        /// <summary>
        /// Тип документа, определяемый по типу шифра в штампе
        /// </summary>
        StampDocumentType StampDocumentType { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        StampOrientationType Orientation { get; }

        /// <summary>
        /// Является ли тип штампа основным
        /// </summary>
        bool IsStampTypeMain { get; }

        /// <summary>
        /// Сжать поля
        /// </summary>
        IEnumerable<bool> CompressFieldsRanges();

        /// <summary>
        /// Вставить подписи
        /// </summary>
        IResultAppCollection<IStampSignature> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures);
    }
}
