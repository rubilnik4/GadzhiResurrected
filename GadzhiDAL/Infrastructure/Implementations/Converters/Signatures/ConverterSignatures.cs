using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;
using NHibernate.Linq;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Signatures
{
    /// <summary>
    /// Преобразование идентификатора с подписью в модель базы банных и трансферную
    /// </summary>
    public static class ConverterSignatures
    {
        /// <summary>
        /// Преобразовать идентификаторы с подписью в модель базы банных
        /// </summary>
        public static IList<SignatureEntity> SignaturesFromDto(IList<SignatureDto> signaturesDto) =>
            signaturesDto?.Select(SignatureFromDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать идентификаторы с подписью в трансферную модель
        /// </summary>
        public static async Task<IList<SignatureDto>> SignaturesFromDto(IList<SignatureEntity> signatureEntities)
        {
            if (signatureEntities == null ) throw new ArgumentNullException(nameof(signatureEntities));
            return await signatureEntities.AsQueryable().
                                           Select(signatire => SignatureToDto(signatire)).
                                           ToListAsync();
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в модель базы банных
        /// </summary>
        public static SignatureMicrostationEntity SignatureMicrostationFromDto(SignatureMicrostationDto signatureMicrostationDto)
        {
            if (signatureMicrostationDto == null) throw new ArgumentNullException(nameof(signatureMicrostationDto));

            var signatureMicrostationEntity = new SignatureMicrostationEntity()
            {
                NameDatabase = signatureMicrostationDto.NameDatabase,
                MicrostationDataBase = signatureMicrostationDto.MicrostationDataBase,
            };

            return signatureMicrostationEntity;
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью Microstation в трансферную модель
        /// </summary>
        public static SignatureMicrostationDto SignatureMicrostationToDto(SignatureMicrostationEntity signatureMicrostationEntity)
        {
            if (signatureMicrostationEntity == null) throw new ArgumentNullException(nameof(signatureMicrostationEntity));

            var signatureMicrostationDto = new SignatureMicrostationDto()
            {
                NameDatabase = signatureMicrostationEntity.NameDatabase,
                MicrostationDataBase = signatureMicrostationEntity.MicrostationDataBase.ToArray(),
            };

            return signatureMicrostationDto;
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью в модель базы банных
        /// </summary>
        private static SignatureEntity SignatureFromDto(SignatureDto signatureDto)
        {
            if (signatureDto == null) throw new ArgumentNullException(nameof(signatureDto));

            var signatureEntity = new SignatureEntity()
            {
                FullName = signatureDto.FullName,
                SignatureJpeg = signatureDto.SignatureJpeg,
            };
            signatureEntity.SetId(signatureDto.Id);

            return signatureEntity;
        }

        /// <summary>
        /// Преобразовать идентификатор с подписью в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(SignatureEntity signatureEntity) =>
            (signatureEntity != null) 
            ? new SignatureDto()
            {
                Id = signatureEntity.Id,
                FullName = signatureEntity.FullName,
                SignatureJpeg = signatureEntity.SignatureJpeg.AsQueryable().ToArray(),
            }
            : throw new ArgumentNullException(nameof(signatureEntity));
    }
}