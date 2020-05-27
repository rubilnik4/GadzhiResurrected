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
        IStampTextField FullCode { get; }

        /// <summary>
        /// Номер текущего листа
        /// </summary>
        IStampTextField CurrentSheet { get; }
    }
}