using System.Collections.Generic;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampTypes;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampCreating
{
    /// <summary>
    /// Проверка штампов Word на корректность
    /// </summary>
    public static class StampValidatingWord
    {
        /// <summary>
        /// Проверить штампы на наличие основного типа
        /// </summary>
        public static IResultAppCollection<TableStampType> ValidateTableStampsByType(IEnumerable<TableStampType> tablesStampWord) =>
            new ResultAppCollection<TableStampType>(tablesStampWord, new ErrorApplication(ErrorApplicationType.StampNotFound,
                                                                                          "Штампы не найдены")).
                ResultValueContinue(tablesStamp => tablesStamp.Count > 0 && StampTypeDefinition.IsStampTypeMain(tablesStamp[0].StampType),
                                    okFunc: tablesStamp => tablesStamp,
                                    badFunc: _ => new ErrorApplication(ErrorApplicationType.StampNotFound, "Основной штамп не найден")).
                ToResultCollection();
    }
}
