using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConvertingLibrary.Models.Converters;

namespace GadzhiConvertingLibrary.Extensions
{
    /// <summary>
    /// Преобразование результирующего ответа модуля конвертации в основной
    /// </summary>
    public static class ResultExtensionsFromApplication
    {
        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации в основной
        /// </summary>      
        public static IResultError ToResultFromApplication(this IResultApplication resultApplication) =>
            ResultApplicationConverter.ToResult(resultApplication);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации со значением в основной
        /// </summary>      
        public static IResultValue<TValue> ToResultValueFromApplication<TValue>(this IResultAppValue<TValue> resultApplication) =>
            ResultApplicationConverter.ToResultValue(resultApplication);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации со значением из основного
        /// </summary>      
        public static IResultAppValue<TValue> ToResultValueApplication<TValue>(this IResultValue<TValue> result) =>
            ResultApplicationConverter.ToResultAppValue(result);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией в основной
        /// </summary>      
        public static IResultCollection<TValue> ToResultCollectionFromApplication<TValue>(this IResultAppCollection<TValue> resultApplication) =>
            ResultApplicationConverter.ToResultCollection(resultApplication);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией из основного
        /// </summary>      
        public static IResultAppCollection<TValue> ToResultCollectionApplication<TValue>(this IResultCollection<TValue> result) =>
            ResultApplicationConverter.ToResultAppCollection(result);
    }
}
