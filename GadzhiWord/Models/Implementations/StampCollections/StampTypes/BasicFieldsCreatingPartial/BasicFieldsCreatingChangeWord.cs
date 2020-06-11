using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiWord.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes.BasicFieldsCreatingPartial
{
    /// <summary>
    /// Фабрика создания базовых полей для извещения изменений Word
    /// </summary>
    public class BasicFieldsCreatingChangeWord : BasicFieldsCreatingWord
    {
        public BasicFieldsCreatingChangeWord(IResultAppValue<IStampTextField> fullCode, IStampFieldsWord stampFieldsWord)
            :base(stampFieldsWord)
        {
            _fullCode = fullCode ?? throw new ArgumentNullException(nameof(fullCode));
        }

        /// <summary>
        /// Поля штампа Word
        /// </summary>
        private readonly IResultAppValue<IStampTextField> _fullCode;

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        public override IResultAppValue<IStampTextField> GetFullCode() => _fullCode;
    }
}