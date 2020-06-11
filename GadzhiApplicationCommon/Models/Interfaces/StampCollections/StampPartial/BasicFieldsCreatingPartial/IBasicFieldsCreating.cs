using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.BasicFieldsCreatingPartial
{
    /// <summary>
    /// Фабрика создания базовых полей
    /// </summary>
    public interface IBasicFieldsCreating
    {
        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        IStampBasicFields StampBasicFields { get; }

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        IResultAppValue<IStampTextField> GetFullCode();

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        IResultAppValue<IStampTextField> GetCurrentSheet();
    }
}