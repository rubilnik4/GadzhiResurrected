using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с базовыми полями штампа
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Получить базовые поля штампа
        /// </summary>
        private IResultAppValue<IStampBasicFields> GetStampBasicFields() =>
            ;
    }
}