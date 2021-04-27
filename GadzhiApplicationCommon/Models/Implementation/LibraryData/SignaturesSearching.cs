using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Helpers;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.LibraryData;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Поиск имен с идентификатором и подписью
    /// </summary>
    public class SignaturesSearching
    {
        public SignaturesSearching(IEnumerable<ISignatureLibraryApp> signaturesLibrary,
                                   Func<IList<SignatureFileRequest>, IResultAppCollection<ISignatureFileApp>> getSignatures)
        {
            _getSignatures = getSignatures;
            _signaturesLibrary = new SortedList<string, ISignatureLibraryApp>(signaturesLibrary.ToDictionary(signature => signature.PersonId,
                                                                                                             signature => signature));
        }

        /// <summary>
        /// Имена с идентификатором и подписи
        /// </summary>
        private readonly SortedList<string, ISignatureLibraryApp> _signaturesLibrary;

        /// <summary>
        /// Функция загрузки подписей из базы данных по идентификаторам
        /// </summary>
        private readonly Func<IList<SignatureFileRequest>, IResultAppCollection<ISignatureFileApp>> _getSignatures;

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
        public ISignatureLibraryApp FindByFullName(string fullName, DepartmentTypeApp departmentType) =>
            PersonsInformation.FindIndex(person => person.SurnameAndDepartmentEqual(fullName, departmentType)).
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
        public IEnumerable<ISignatureLibraryApp> FindByFullNames(IEnumerable<string> fullNames, DepartmentTypeApp departmentType = DepartmentTypeApp.Unknown) =>
            fullNames.Select(fullName => FindByFullName(fullName, departmentType)).
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
            FindByFullNameOrRandom(fullName, FindById(personId)?.PersonInformation.DepartmentType ?? DepartmentTypeApp.Unknown);

        /// <summary>
        /// Найти подпись по имени или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByFullNameOrRandom(string fullName, DepartmentTypeApp departmentType) =>
            new ResultAppValue<ISignatureLibraryApp>(FindByFullName(fullName, departmentType),
                                                     new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись по имени {fullName} не найдена")).
            ResultValueBadBind(_ => new ResultAppValue<ISignatureLibraryApp>(GetRandomSignature(),
                                                                             new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                  "База подписей пуста")));

        /// <summary>
        /// Найти подпись по информации о пользователе или получить случайную
        /// </summary>
        public IResultAppValue<ISignatureLibraryApp> FindByPersonInformationOrRandom(PersonInformationApp personInformation) =>
            new ResultAppValue<ISignatureLibraryApp>(FindByPersonInformation(personInformation),
                                                  new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись по имени {personInformation.FullName} не найдена")).
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
            ResultValueOk(ids => ids.Select(id => new SignatureFileRequest(id, false))).
            ResultValueOkBind(GetSignaturesByIds).
            ToResultCollection();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам с возможностью поворота
        /// </summary>
        public IResultAppCollection<ISignatureFileApp> GetSignaturesByIds(IEnumerable<SignatureFileRequest> personRequests) =>
            new ResultAppCollection<SignatureFileRequest>(personRequests).
            ResultValueOkBind(requests => _getSignatures(requests).
                                          ResultValueOkBind(signaturesFile => SignatureLeftJoinWithDataBase(requests, signaturesFile))).
            ToResultCollection();

        /// <summary>
        /// Проверить отдел по его типу
        /// </summary>
        public DepartmentTypeApp CheckDepartmentAccordingToType(DepartmentTypeApp departmentType, PersonDepartmentType personDepartmentType) =>
            personDepartmentType switch
            {
                PersonDepartmentType.ChiefProject => DepartmentTypeApp.Gip,
                _ => departmentType,
            };

        /// <summary>
        /// Связать значения подписей с базой данных один к одному
        /// </summary>
        private static IResultAppCollection<ISignatureFileApp> SignatureLeftJoinWithDataBase(IEnumerable<SignatureFileRequest> personRequests,
                                                                                             IList<ISignatureFileApp> signaturesFile) =>
            personRequests.
            Select(personRequest => signaturesFile.FirstOrDefault(signatureFile => signatureFile.PersonId == personRequest.PersonId &&
                                                                                   signatureFile.IsVerticalImage == personRequest.IsVerticalImage)).
            Select(signatureFile => new ResultAppValue<ISignatureFileApp>(signatureFile, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                           "Подпись в базе не найдена"))).
            Select(signatureResult => (IResultAppValue<ISignatureFileApp>)signatureResult).
            ToResultCollection();
    }
}