using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDAL.Entities.Signatures.Components;

namespace GadzhiDAL.Entities.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class SignatureEntity: ISignatureFileDataBase<PersonInformationComponent>
    {
        public SignatureEntity()
        { }

        public SignatureEntity(string personId, PersonInformationComponent personInformation, IList<byte> signatureSource)
        {
            PersonId = personId;
            PersonInformation = personInformation;
            SignatureSource = signatureSource;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string PersonId { get; protected set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public virtual PersonInformationComponent PersonInformation { get; protected set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>       
        public virtual IList<byte> SignatureSource { get; protected set; }
    }
}