using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Signatures;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOServer.TransferModels.Signatures;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Получение и запись из БД подписей и идентификаторов
    /// </summary>
    public class SignaturesServerService : ISignaturesServerService
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        public SignaturesServerService(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto)
        {
            var signaturesEntity = ConverterSignatures.SignaturesFromDto(signaturesDto);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            foreach (var signatureEntity in signaturesEntity)
            {
                await unitOfWork.Session.SaveOrUpdateAsync(signatureEntity);
            }

            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var signatureEntities = await unitOfWork.Session.Query<SignatureEntity>().
                                                     Where(signature => ids.Contains(signature.Id)).
                                                     ToListAsync();
            var signaturesDto = await ConverterSignatures.SignaturesFromDto(signatureEntities);

            await unitOfWork.CommitAsync();

            return signaturesDto;
        }

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>      
        public async Task<SignatureMicrostationDto> GetSignaturesMicrostation()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var signatureMicrostationEntity = await unitOfWork.Session.Query<SignatureMicrostationEntity>().FirstOrDefaultAsync();
            var signatureMicrostationDto = ConverterSignatures.SignatureMicrostationToDto(signatureMicrostationEntity);

            await unitOfWork.CommitAsync();

            return signatureMicrostationDto;
        }

        /// <summary>
        /// Записать подписи Microstation в базу данных
        /// </summary>      
        public async Task UploadSignaturesMicrostation(SignatureMicrostationDto signatureMicrostationDto)
        {
            var signatureMicrostationEntity = ConverterSignatures.SignatureMicrostationFromDto(signatureMicrostationDto);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await unitOfWork.Session.SaveOrUpdateAsync(signatureMicrostationEntity);

            await unitOfWork.CommitAsync();
        }
    }
}