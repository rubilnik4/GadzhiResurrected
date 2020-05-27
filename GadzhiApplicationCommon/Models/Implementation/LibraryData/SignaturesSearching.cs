using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Helpers;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Поиск имен с идентификатором и подписью
    /// </summary>
    public class SignaturesSearching
    {
        /// <summary>
        /// Имена с идентификатором и подписи
        /// </summary>
        private readonly SortedList<string, ISignatureLibraryApp> _signaturesLibrary;

        /// <summary>
        /// Функция загрузки подписей из базы данных по идентификаторам
        /// </summary>
        private readonly Func<IEnumerable<string>, IList<ISignatureFileApp>> _getSignatures;

        public SignaturesSearching(IEnumerable<ISignatureLibraryApp> signaturesLibrary,
                                          Func<IEnumerable<string>, IList<ISignatureFileApp>> getSignatures)
        {
            if (signaturesLibrary == null) throw new ArgumentNullException(nameof(signaturesLibrary));
            _getSignatures = getSignatures ?? throw new ArgumentNullException(nameof(getSignatures));

            _signaturesLibrary = new SortedList<string, ISignatureLibraryApp>(signaturesLibrary.ToDictionary(signature => signature.PersonId,
                                                                                                         signature => signature));
        }

        /// <summary>
        /// Список имен
        /// </summary>
        private List<PersonInformationApp> _personsInformation;

        /// <summary>
        /// Список по
        /// </summary>
        private List<PersonInformationApp> PersonsInformation => _personsInformation ??= _signaturesLibrary.Values.
                                                               Select(signature => signature.PersonInformation).
                                                               ToList();

        /// <summary>
        /// Найти подпись по идентификатору
        /// </summary>
        public ISignatureLibraryApp FindById(string id) => _signaturesLibrary.TryGetValue(id, out var signatureFind)
                                                       ? signatureFind
                                                       : null;

        /// <summary>
        /// Найти подписи по идентификаторам
        /// </summary>
        public IEnumerable<ISignatureLibraryApp> FindByIds(IEnumerable<string> ids) =>
            ids?.Where(id => _signaturesLibrary.ContainsKey(id)).
                 Select(id => _signaturesLibrary[id]);

        /// <summary>
        /// Найти подпись по имени
        /// </summary>
        public ISignatureLibraryApp FindByFullName(string fullName, string department) =>
            PersonsInformation.FindIndex(person => person.SurnameAndDepartmentEqual(fullName, department)).
            WhereBad(foundIndex => foundIndex > -1,
                badFunc: _ => PersonsInformation.FindIndex(person => person.SurnameEqual(fullName))).
            WhereContinue(foundIndex => foundIndex > -1,
                okFunc: foundIndex => _signaturesLibrary.Values[foundIndex],
                badFunc: foundIndex => null);

        /// <summary>
        /// Найти подпись по информации о пользователе
        /// </summary>
        public ISignatureLibraryApp FindByPersonInformation(PersonInformationApp personInformation) =>
            PersonsInformation.IndexOf(personInformation).
            WhereContinue(foundIndex => foundIndex > -1,
                okFunc: foundIndex => _signaturesLibrary.Values[foundIndex],
                badFunc: foundIndex => null);

        /// <summary>
        /// Найти подписи по именам
        /// </summary>
        public IEnumerable<ISignatureLibraryApp> FindByFullNames(IEnumerable<string> fullNames, string department = "") =>
            fullNames.Select(fullName => FindByFullName(fullName, department)).
                      Where(signature => signature != null);

        /// <summary>
        /// Получить случайную подпись
        /// </summary>
        public ISignatureLibraryApp GetRandomSignature() =>
            _signaturesLibrary.Count > 0
                ? _signaturesLibrary.Values[RandomInstance.RandomNumber(_signaturesLibrary.Values.Count)]
                : null;

        /// <summary>
        /// Найти подпись по имени или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByFullNameOrRandom(string fullName, string personId) =>
            new ResultAppValue<ISignatureLibraryApp>(FindByFullName(fullName, FindById(personId)?.PersonInformation.Department),
                                                     new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись  по имени {fullName} не найдена")).
            ResultValueBadBind(_ => new ResultAppValue<ISignatureLibraryApp>(GetRandomSignature(),
                                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                  "База подписей пуста")));

        /// <summary>
        /// Найти подпись по информации о пользователе или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByPersonInformationOrRandom(PersonInformationApp personInformation) =>
            new ResultAppValue<ISignatureLibraryApp>(FindByPersonInformation(personInformation),
                                                  new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись  по имени {personInformation.FullName} не найдена")).
                ResultValueBadBind(_ => new ResultAppValue<ISignatureLibraryApp>(GetRandomSignature(),
                                                                              new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                   "База подписей пуста")));

        /// <summary>
        /// Найти подпись по идентификатору или имени или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByIdOrFullNameOrRandom(string id, string fullName, string personId) =>
            new ResultAppValue<ISignatureLibraryApp>(FindById(id), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                    $"Подпись по идентификатору {id} не найдена")).
            ResultValueBadBind(_ => FindByFullNameOrRandom(fullName, personId));

        /// <summary>
        /// Найти подпись по идентификатору или информации о пользователе или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByIdOrPersonInformationOrRandom(string id, PersonInformationApp personInformation) =>
            new ResultAppValue<ISignatureLibraryApp>(FindById(id), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                     $"Подпись по идентификатору {id} не найдена")).
                ResultValueBadBind(_ => FindByPersonInformationOrRandom(personInformation));

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>
        public IResultAppCollection<ISignatureFileApp> GetSignaturesByIds(IEnumerable<string> personsId) =>
            new ResultAppCollection<string>(personsId).
            ResultValueOkBind(ids => new ResultAppCollection<ISignatureFileApp>(_getSignatures(ids), new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                                       "Подписи в базе не найдены")).
                                     ResultValueOkBind(signaturesFile => SignatureLeftJoinWithDataBase(ids, signaturesFile))).
            ToResultCollection();

        /// <summary>
        /// Проверить отдел по его типу
        /// </summary>
        public string CheckDepartmentAccordingToType(string department, PersonDepartmentType departmentType) =>
            departmentType switch
            {
                PersonDepartmentType.ChiefProject => "ГИПы",
                _ => department,
            };

        /// <summary>
        /// Связать значения подписей с базой данных один к одному
        /// </summary>
        private static IResultAppCollection<ISignatureFileApp> SignatureLeftJoinWithDataBase(IEnumerable<string> personIds, IList<ISignatureFileApp> signaturesFile) =>
            personIds.
            Select(id => signaturesFile.FirstOrDefault(signatureFile => signatureFile.PersonId == id)).
            Select(signatureFile => new ResultAppValue<ISignatureFileApp>(signatureFile, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                           "Подпись в базе не найдена"))).
            Select(signatureResult => (IResultAppValue<ISignatureFileApp>)signatureResult).
            ToResultCollection();
    }
}