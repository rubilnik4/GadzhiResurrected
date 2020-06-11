using System;
using System.Collections.Generic;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiWord.Models.Interfaces.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Фабрика создания подписей Word
    /// </summary>
    public partial class SignatureCreatingWord: SignatureCreating
    {
        public SignatureCreatingWord(ITableElementWord tableStamp, IResultAppValue<ITableElementWord> tableApprovalPerformers,
                                     IStampFieldsWord stampFieldsWord, SignaturesSearching signaturesSearching,
                                     StampDocumentType stampDocumentType , string personId)
        {
            _tableStamp = tableStamp ?? throw new ArgumentNullException(nameof(tableStamp));
            _tableApprovalPerformers = tableApprovalPerformers ?? throw new ArgumentNullException(nameof(tableApprovalPerformers));
            _stampFieldsWord = stampFieldsWord ?? throw new ArgumentNullException(nameof(stampFieldsWord));
            _signaturesSearching = signaturesSearching ?? throw new ArgumentNullException(nameof(signaturesSearching));
            _stampDocumentType = stampDocumentType;
            _personId = personId;
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        private readonly ITableElementWord _tableStamp;

        /// <summary>
        /// Элемент таблица согласования списка исполнителей
        /// </summary>
        private readonly IResultAppValue<ITableElementWord> _tableApprovalPerformers;

        /// <summary>
        /// Поля штампа Word
        /// </summary>
        private readonly IStampFieldsWord _stampFieldsWord;

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        private readonly SignaturesSearching _signaturesSearching;

        /// <summary>
        /// Тип документа, определяемый по типу шифра в штампе
        /// </summary>
        private readonly StampDocumentType _stampDocumentType;

        /// <summary>
        /// Идентификатор личной подписи
        /// </summary>
        private readonly string  _personId;

        /// <summary>
        /// Получить информацию об ответственном лице по имени
        /// </summary>      
        private IResultAppValue<ISignatureLibraryApp> GetSignatureInformation(string personName, string personId,
                                                                              PersonDepartmentType personDepartmentType) =>
            _signaturesSearching.FindById(personId)?.PersonInformation.DepartmentType.
            Map(departmentType => _signaturesSearching.CheckDepartmentAccordingToType(departmentType, personDepartmentType)).
            Map(departmentChecked => _signaturesSearching.FindByFullNameOrRandom(personName, departmentChecked));
    }
}