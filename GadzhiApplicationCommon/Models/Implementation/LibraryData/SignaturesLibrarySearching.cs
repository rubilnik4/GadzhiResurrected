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
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

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
        private readonly SortedList<string, ISignatureLibrary> _signaturesLibrary;

        /// <summary>
        /// Функция загрузки подписей из базы данных по идентификаторам
        /// </summary>
        private readonly Func<IEnumerable<string>, IEnumerable<ISignatureFile>> _getSignatures;

        public SignaturesLibrarySearching(IEnumerable<ISignatureLibrary> signaturesLibrary,
                                          Func<IEnumerable<string>, IEnumerable<ISignatureFile>> getSignatures)
        {
            if (signaturesLibrary == null) throw new ArgumentNullException(nameof(signaturesLibrary));

            _signaturesLibrary = new SortedList<string, ISignatureLibrary>(signaturesLibrary.ToDictionary(signature => signature.PersonId,
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
                                                         Select(signature => signature.PersonName.ToLowerCaseCurrentCulture()).
                                                         ToList();

        /// <summary>
        /// Найти подпись по идентификатору
        /// </summary>
        public ISignatureLibrary FindById(string id) => _signaturesLibrary.TryGetValue(id, out var signatureFind)
                                                       ? signatureFind
                                                       : null;

        /// <summary>
        /// Найти подписи по идентификаторам
        /// </summary>
        public IEnumerable<ISignatureLibrary> FindByIds(IEnumerable<string> ids) =>
            ids?.Where(id => _signaturesLibrary.ContainsKey(id)).
                 Select(id => _signaturesLibrary[id]);

        /// <summary>
        /// Найти подпись по имени
        /// </summary>
        public ISignatureLibrary FindByFullName(string fullName) =>
            FullNames.IndexOf(fullName.ToLowerCaseCurrentCulture()).
            WhereContinue(foundIndex => foundIndex > -1,
                okFunc: foundIndex => _signaturesLibrary.Values[foundIndex],
                badFunc: foundIndex => null);

        /// <summary>
        /// Найти подписи по именам
        /// </summary>
        public IEnumerable<ISignatureLibrary> FindByFullNames(IEnumerable<string> fullNames) =>
            fullNames?.Select(fullname => fullname.ToLowerCaseCurrentCulture()).
            Map(fullNamesLower => fullNamesLower.Select(FindByFullName).
                                                 Where(signature => signature != null));

        /// <summary>
        /// Получить случайную подпись
        /// </summary>
        public ISignatureLibrary GetRandomSignature() =>
            _signaturesLibrary.Count > 0
                ? _signaturesLibrary.Values[RandomInstance.RandomNumber(_signaturesLibrary.Values.Count)]
                : null;

        /// <summary>
        /// Найти подпись по имени или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibrary> FindByFullNameOrRandom(string fullName) =>
            new ResultAppValue<ISignatureLibrary>(FindByFullName(fullName), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                $"Подпись  по имени {fullName} не найдена")).
                ResultValueBadBind(_ => new ResultAppValue<ISignatureLibrary>(GetRandomSignature(),
                                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                  "База подписей пуста")));

        /// <summary>
        /// Найти подпись по идентификатору или имени или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibrary> FindByIdOrFullNameOrRandom(string id, string fullName) =>
            new ResultAppValue<ISignatureLibrary>(FindById(id), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                    $"Подпись по идентификатору {id} не найдена")).
            ResultValueBadBind(_ => FindByFullNameOrRandom(fullName)).
            ResultValueBadBind(_ => new ResultAppValue<ISignatureLibrary>(GetRandomSignature(),
                                                                          new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                               "База подписей пуста")));

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>
        public IEnumerable<ISignatureFile> GetSignaturesByIds(IEnumerable<string> ids) => _getSignatures(ids);
    }
}