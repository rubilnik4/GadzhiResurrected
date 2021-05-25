using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Entities.Signatures.Components;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.DataFile
{
    /// <summary>
    /// Преобразование идентификатора с подписью в модель базы банных и трансферную
    /// </summary>
    public static class ConverterDataFile
    {
        /// <summary>
        /// Преобразовать идентификаторы с подписью в модель базы банных
        /// </summary>
        public static IList<SignatureEntity> SignaturesFromDto(IList<SignatureDto> signaturesDto) =>
            signaturesDto.Select(SignatureFromDto).ToList();

        /// <summary>
        /// Преобразовать идентификаторы с подписью в трансферную модель
        /// </summary>
        public static IList<SignatureDto> SignaturesToDto(IList<SignatureEntity> signatureEntities, bool signatureLoad) =>
            signatureEntities.Select(signature => SignatureToDto(signature, signatureLoad)).ToList();

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в модель базы банных
        /// </summary>
        public static MicrostationDataFileEntity MicrostationDataFileFromDto(MicrostationDataFileDto microstationDataFileDto, string idDataFile)
        {
            var signatureMicrostationEntity = new MicrostationDataFileEntity
            {
                NameDatabase = microstationDataFileDto.NameDatabase,
                MicrostationDataBase = microstationDataFileDto.MicrostationDataBase,
            };
            signatureMicrostationEntity.SetId(idDataFile);
            return signatureMicrostationEntity;
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFileDto SignatureMicrostationToDto(MicrostationDataFileEntity microstationDataFileEntity)
        {
            var signatureMicrostationDto = new MicrostationDataFileDto
            {
                NameDatabase = microstationDataFileEntity.NameDatabase,
                MicrostationDataBase = microstationDataFileEntity.MicrostationDataBase.ToArray(),
            };
            return signatureMicrostationDto;
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью в модель базы банных
        /// </summary>
        private static SignatureEntity SignatureFromDto(SignatureDto signatureDto)
        {
            var signatureEntity = new SignatureEntity
            {
                PersonInformation = PersonInformationFromDto(signatureDto.PersonInformation),
                SignatureJpeg = signatureDto.SignatureJpeg,
            };
            signatureEntity.SetId(signatureDto.PersonId);
            return signatureEntity;
        }

        private static PersonInformationComponent PersonInformationFromDto(PersonInformationDto personInformation) =>
            new PersonInformationComponent
            {
                Surname = personInformation.Surname,
                Name = personInformation.Name,
                Patronymic = personInformation.Patronymic,
                DepartmentType = personInformation.DepartmentType,
            };

        /// <summary>
        /// Преобразовать идентификатор с подписью в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(SignatureEntity signatureEntity, bool signatureLoad) =>
            new SignatureDto(signatureEntity.Id, PersonInformationToDto(signatureEntity.PersonInformation),
                             signatureLoad ? signatureEntity.SignatureJpeg.ToArray() : null);

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        private static PersonInformationDto PersonInformationToDto(PersonInformationComponent personInformation) =>
            new PersonInformationDto(personInformation.Surname, personInformation.Name,
                                     personInformation.Patronymic, personInformation.DepartmentType);
    }
}