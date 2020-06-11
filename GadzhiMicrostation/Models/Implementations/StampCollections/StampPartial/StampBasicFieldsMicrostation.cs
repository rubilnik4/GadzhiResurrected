using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.BasicFieldsCreatingPartial;


namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа Microstation
    /// </summary>
    public partial class StampMicrostation
    {
        /// <summary>
        /// Фабрика создания базовых полей Microstation
        /// </summary>
        protected override IBasicFieldsCreating BasicFieldsCreating => new BasicFieldsCreatingMicrostation(this);

        /// <summary>
        /// Получить базовые поля штампа Microstation
        /// </summary>
        protected override IStampBasicFields GetStampBasicFields() => BasicFieldsCreating.StampBasicFields;
    }
}