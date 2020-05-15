using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

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
            signaturesDto?.Select(SignaturesFromDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать идентификатор с подписью в модель базы банных
        /// </summary>
        public static SignatureEntity SignaturesFromDto(SignatureDto signatureDto)
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

    }
}