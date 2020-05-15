using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Infrastructure.Implementations.Converters.Signatures;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOServer.TransferModels.Signatures;
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