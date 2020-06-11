using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.BasicFieldsCreatingPartial;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Базовые поля штампа
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Фабрика создания подписей Word
        /// </summary>
        protected abstract IBasicFieldsCreating BasicFieldsCreating { get; }

        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        protected abstract IStampBasicFields GetStampBasicFields();
    }
}