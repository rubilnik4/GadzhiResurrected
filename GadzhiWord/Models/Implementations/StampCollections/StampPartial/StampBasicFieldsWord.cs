using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial.BasicFieldsCreatingPartial;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Фабрика создания базовых полей Word
        /// </summary>
        protected override IBasicFieldsCreating BasicFieldsCreating => new BasicFieldsCreatingWord(this);

        /// <summary>
        /// Получить базовые поля штампа Word
        /// </summary>
        protected override IStampBasicFields GetStampBasicFields() => BasicFieldsCreating.StampBasicFields;
    }
}