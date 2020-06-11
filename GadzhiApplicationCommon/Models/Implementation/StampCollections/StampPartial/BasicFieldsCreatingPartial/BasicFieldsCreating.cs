using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.BasicFieldsCreatingPartial;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.BasicFieldsCreatingPartial
{
    /// <summary>
    /// Фабрика создания базовых полей
    /// </summary>
    public abstract class BasicFieldsCreating: IBasicFieldsCreating
    {
        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        public IStampBasicFields StampBasicFields => new StampBasicFields(GetFullCode(), GetCurrentSheet());

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        public abstract IResultAppValue<IStampTextField> GetFullCode();

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        public abstract IResultAppValue<IStampTextField> GetCurrentSheet();
    }
}