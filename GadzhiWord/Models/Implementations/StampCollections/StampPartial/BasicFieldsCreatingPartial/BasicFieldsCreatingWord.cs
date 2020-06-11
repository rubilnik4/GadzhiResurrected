using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.BasicFieldsCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.BasicFieldsCreatingPartial
{
    /// <summary>
    /// Фабрика создания базовых полей Word
    /// </summary>
    public class BasicFieldsCreatingWord : BasicFieldsCreating
    {
        public BasicFieldsCreatingWord(IStampFieldsWord stampFieldsWord)
        {
            _stampFieldsWord = stampFieldsWord ?? throw new ArgumentNullException(nameof(stampFieldsWord));
        }

        /// <summary>
        /// Поля штампа Word
        /// </summary>
        private readonly IStampFieldsWord _stampFieldsWord;

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        public override IResultAppValue<IStampTextField> GetFullCode() =>
            new ResultAppValue<IStampTextField>(_stampFieldsWord.GetFieldsByType(StampFieldType.FullRow).FirstOrDefault(),
                                                new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра в таблице не найдено"));

        /// <summary>
        /// Получить номер текущего листа
        /// </summary>
        public override IResultAppValue<IStampTextField> GetCurrentSheet() =>
            new ResultAppValue<IStampTextField>(_stampFieldsWord.GetFieldsByType(StampFieldType.CurrentSheet).FirstOrDefault(),
                                                new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле текущего листа в таблице не найдено"));
    }
}