using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Infrastructure.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Определение типа действия
    /// </summary>
    public static class SignaturesActionType
    {
        /// <summary>
        /// Получить тип действия
        /// </summary>
        public static StampActionType GetStampActionType(string actionType) =>
            actionType.ToLowerInvariant().Trim().
            Map(action => action switch
            {
                _ when action.StartsWith("разраб") || action.StartsWith("исполн") || action.StartsWith("состав") => StampActionType.Developer,
                _ when action.StartsWith("провер") || action.StartsWith("контр") => StampActionType.Verifier,
                _ when action.StartsWith("гип") => StampActionType.Gip,
                _ => StampActionType.Unknown,
            });

        /// <summary>
        /// Получить подпись согласно типа действия
        /// </summary>
        public static string GetPersonIdByActionType(string personAttributeId, bool useDefaultSignature,
                                                     string personId, string actionType) =>
            useDefaultSignature && GetStampActionType(actionType) == StampActionType.Developer
                ? personId
                : personAttributeId;
    }
}