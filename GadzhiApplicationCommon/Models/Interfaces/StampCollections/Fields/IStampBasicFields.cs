using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields
{
    /// <summary>
    /// Обязательные поля штампа
    /// </summary>
    public interface IStampBasicFields
    {
        /// <summary>
        /// Шифр
        /// </summary>
        IResultAppValue<IStampTextField> FullCode { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        IResultAppValue<IStampTextField> CurrentSheet { get; }

        /// <summary>
        /// Номер текущего листа в числовом формате
        /// </summary>
        IResultAppValue<int> CurrentSheetNumber { get; }
    }
}