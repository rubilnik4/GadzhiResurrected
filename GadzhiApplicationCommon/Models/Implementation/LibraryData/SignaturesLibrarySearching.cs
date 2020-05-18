using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Helpers;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Поиск имен с идентификатором и подписью
    /// </summary>
    public class SignaturesLibrarySearching
    {
        /// <summary>
        /// Имена с идентификатором и подписи
        /// </summary>
        private readonly SortedList<string, SignatureLibrary> _signaturesLibrary;

        /// <summary>
        /// Функция загрузки подписей из базы данных по идентификаторам
        /// </summary>
        private readonly Func<IEnumerable<string>, IEnumerable<SignatureLibrary>> _getSignatures;

        public SignaturesLibrarySearching(IEnumerable<SignatureLibrary> signaturesLibrary,
                                          Func<IEnumerable<string>, IEnumerable<SignatureLibrary>> getSignatures)
        {
            if (signaturesLibrary == null) throw new ArgumentNullException(nameof(signaturesLibrary));

            _signaturesLibrary = new SortedList<string, SignatureLibrary>(signaturesLibrary.ToDictionary(signature => signature.Id,
                                                                                                         signature => signature));
            _getSignatures = getSignatures ?? throw new ArgumentNullException(nameof(getSignatures));
        }

        /// <summary>
        /// Список имен
        /// </summary>
        private List<string> _fullNames;

        /// <summary>
        /// Список имен
        /// </summary>
        private List<string> FullNames => _fullNames ??= _signaturesLibrary.Values.
                                                         Select(signature => signature.Fullname.ToLowerCaseCurrentCulture()).
                                                         ToList();

        /// <summary>
        /// Найти подпись по идентификатору
        /// </summary>
        public SignatureLibrary FindById(string id) => _signaturesLibrary.TryGetValue(id, out var signatureFind)
                                                       ? signatureFind
                                                       : null;

        /// <summary>
        /// Найти подписи по идентификаторам
        /// </summary>
        public IEnumerable<SignatureLibrary> FindByIds(IEnumerable<string> ids) =>
            ids?.Where(id => _signaturesLibrary.ContainsKey(id)).
                 Select(id => _signaturesLibrary[id]);

        /// <summary>
        /// Найти подпись по имени
        /// </summary>
        public SignatureLibrary FindByFullName(string fullName) =>
            FullNames.IndexOf(fullName.ToLowerCaseCurrentCulture()).
            WhereContinue(foundIndex => foundIndex > -1,
                okFunc: foundIndex => _signaturesLibrary.Values[foundIndex],
                badFunc: foundIndex => null);

        /// <summary>
        /// Найти подписи по именам
        /// </summary>
        public IEnumerable<SignatureLibrary> FindByFullNames(IEnumerable<string> fullNames) =>
            fullNames?.Select(fullname => fullname.ToLowerCaseCurrentCulture()).
            Map(fullNamesLower => fullNamesLower.Select(FindByFullName).
                                                 Where(signature => signature != null));

        /// <summary>
        /// Получить случайную подпись
        /// </summary>
        public SignatureLibrary GetRandomSignature() =>
            _signaturesLibrary.Count > 0
                ? _signaturesLibrary.Values[RandomInstance.RandomNumber(_signaturesLibrary.Values.Count)]
                : null;

        /// <summary>
        /// Найти подпись по имени или получить случайную
        /// </summary>
        public IResultAppValue<SignatureLibrary> FindByFullNameOrRandom(string fullName) =>
            new ResultAppValue<SignatureLibrary>(FindByFullName(fullName), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                $"Подпись  по имени {fullName} не найдена")).
                ResultValueBadBind(_ => new ResultAppValue<SignatureLibrary>(GetRandomSignature(),
                                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                  "База подписей пуста")));

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>
        public IEnumerable<SignatureLibrary> GetSignaturesByIds(IEnumerable<string> ids) => _getSignatures(ids);
    }
}