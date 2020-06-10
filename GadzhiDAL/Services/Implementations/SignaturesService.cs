using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.DataFile;
using GadzhiDAL.Models.Implementations;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Получение и запись из БД подписей и идентификаторов
    /// </summary>
    public class SignaturesService : ISignaturesService
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        public SignaturesService(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var signatureEntities = await unitOfWork.Session.Query<SignatureEntity>().
                                                     OrderBy(signature => signature.PersonInformation.Surname).
                                                     ThenBy(signature => signature.PersonInformation.Name).
                                                     ToListAsync();
            var signaturesDto = ConverterDataFile.SignaturesToDto(signatureEntities, false);

            await unitOfWork.CommitAsync();

            return signaturesDto;
        }

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>      
        public async Task<IList<DepartmentType>> GetSignaturesDepartments()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var departments = await unitOfWork.Session.Query<SignatureEntity>().
                                               Select(signature => signature.PersonInformation.DepartmentType).
                                               Distinct().
                                               ToListAsync();
            await unitOfWork.CommitAsync();

            return departments;
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
            var signaturesDto = ConverterDataFile.SignaturesToDto(signatureEntities, true);

            await unitOfWork.CommitAsync();

            return signaturesDto;
        }

        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto)
        {
            var signaturesEntity = ConverterDataFile.SignaturesFromDto(signaturesDto);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            foreach (var signatureEntity in signaturesEntity)
            {
                await unitOfWork.Session.SaveOrUpdateAsync(signatureEntity);
            }

            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получить данные Microstation из базы данных
        /// </summary>      
        public async Task<MicrostationDataFileDto> GetMicrostationDataFile(string idDataFile)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var signatureMicrostationEntity = await unitOfWork.Session.LoadAsync<MicrostationDataFileEntity>(idDataFile);
            var signatureMicrostationDto = ConverterDataFile.SignatureMicrostationToDto(signatureMicrostationEntity);

            await unitOfWork.CommitAsync();

            return signatureMicrostationDto;
        }

        /// <summary>
        /// Записать данные Microstation в базу данных
        /// </summary>      
        public async Task UploadMicrostationDataFile(MicrostationDataFileDto microstationDataFileDto, string idDataFile)
        {
            var signatureMicrostationEntity = ConverterDataFile.MicrostationDataFileFromDto(microstationDataFileDto, idDataFile);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await unitOfWork.Session.SaveOrUpdateAsync(signatureMicrostationEntity);

            await unitOfWork.CommitAsync();
        }
    }
}