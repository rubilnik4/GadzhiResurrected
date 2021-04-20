using System;
using System.Collections.Generic;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiWord.Models.Interfaces.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Фабрика создания подписей Word
    /// </summary>
    public partial class SignatureCreatingWord : SignatureCreating
    {
        public SignatureCreatingWord(ITableElementWord tableStamp, StampIdentifier stampIdentifier, IResultAppValue<ITableElementWord> tableApprovalPerformers,
                                     IResultAppValue<ITableElementWord> tableApprovalChief,
                                     IStampFieldsWord stampFieldsWord, StampDocumentType stampDocumentType,
                                     SignaturesSearching signaturesSearching, string personId)
            : base(signaturesSearching, personId)
        {
            _tableStamp = tableStamp ?? throw new ArgumentNullException(nameof(tableStamp));
            _stampIdentifier = stampIdentifier;
            _tableApprovalPerformers = tableApprovalPerformers ?? throw new ArgumentNullException(nameof(tableApprovalPerformers));
            _tableApprovalChief = tableApprovalChief ?? throw new ArgumentNullException(nameof(tableApprovalChief));
            _stampFieldsWord = stampFieldsWord ?? throw new ArgumentNullException(nameof(stampFieldsWord));
            _stampDocumentType = stampDocumentType;
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        private readonly ITableElementWord _tableStamp;

        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        private readonly StampIdentifier _stampIdentifier;

        /// <summary>
        /// Элемент таблица согласования списка исполнителей
        /// </summary>
        private readonly IResultAppValue<ITableElementWord> _tableApprovalPerformers;

        /// <summary>
        /// Элемент таблица согласования с директорами
        /// </summary>
        private readonly IResultAppValue<ITableElementWord> _tableApprovalChief;

        /// <summary>
        /// Поля штампа Word
        /// </summary>
        private readonly IStampFieldsWord _stampFieldsWord;

        /// <summary>
        /// Тип документа, определяемый по типу шифра в штампе
        /// </summary>
        private readonly StampDocumentType _stampDocumentType;

        /// <summary>
        /// Получить информацию об ответственном лице по имени
        /// </summary>      
        private IResultAppValue<ISignatureLibraryApp> GetSignatureInformation(string personName, string personId,
                                                                              PersonDepartmentType personDepartmentType) =>
            new ResultAppValue<ISignatureLibraryApp>(SignaturesSearching.FindById(personId),
                                                     new ErrorApplication(ErrorApplicationType.SignatureNotFound, $"Подпись {personName} не найдена")).
            ResultValueOk(signature => signature.PersonInformation.DepartmentType).
            ResultValueOk(departmentType => SignaturesSearching.CheckDepartmentAccordingToType(departmentType, personDepartmentType)).
            ResultValueOkBind(departmentChecked => SignaturesSearching.FindByFullNameOrRandom(personName, departmentChecked));
    }
}