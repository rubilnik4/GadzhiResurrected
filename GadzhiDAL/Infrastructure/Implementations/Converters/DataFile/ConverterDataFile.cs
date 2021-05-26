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
        public static IList<SignatureDto> SignaturesToDto(IList<SignatureEntity> signatureEntities) =>
            signatureEntities.Select(SignatureToDto).ToList();

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в модель базы банных
        /// </summary>
        public static MicrostationDataFileEntity MicrostationDataFileFromDto(MicrostationDataFileDto microstationDataFileDto,
                                                                             string idDataFile) =>
            new MicrostationDataFileEntity(idDataFile, microstationDataFileDto.NameDatabase,
                                           microstationDataFileDto.MicrostationDataBase);

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFileDto SignatureMicrostationToDto(MicrostationDataFileEntity microstationDataFileEntity) =>
            new MicrostationDataFileDto(microstationDataFileEntity.NameDatabase,
                                        microstationDataFileEntity.MicrostationDataBase.ToArray());

        /// <summary>
        /// Преобразовать идентификатор с подписью в модель базы банных
        /// </summary>
        private static SignatureEntity SignatureFromDto(SignatureDto signatureDto) =>
            new SignatureEntity(signatureDto.PersonId,
                                PersonInformationFromDto(signatureDto.PersonInformation),
                                signatureDto.SignatureSource);
        /// <summary>
        /// Преобразовать информацию о пользователе в транспортную модель
        /// </summary>
        private static PersonInformationComponent PersonInformationFromDto(PersonInformationDto personInformation) =>
            new PersonInformationComponent(personInformation.Surname, personInformation.Name,
                                           personInformation.Patronymic, personInformation.DepartmentType);

        /// <summary>
        /// Преобразовать идентификатор с подписью в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(SignatureEntity signatureEntity) =>
            new SignatureDto(signatureEntity.PersonId, PersonInformationToDto(signatureEntity.PersonInformation),
                             signatureEntity.SignatureSource.ToArray());

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        private static PersonInformationDto PersonInformationToDto(PersonInformationComponent personInformation) =>
            new PersonInformationDto(personInformation.Surname, personInformation.Name,
                                     personInformation.Patronymic, personInformation.DepartmentType);
    }
}