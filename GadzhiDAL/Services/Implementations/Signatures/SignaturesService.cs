﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.DataFile;
using GadzhiDAL.Models.Implementations;
using GadzhiDAL.Services.Interfaces;
using GadzhiDAL.Services.Interfaces.Signatures;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.Signatures
{
    /// <summary>
    /// Получение и запись из БД подписей и идентификаторов
    /// </summary>
    public class SignaturesService : ISignaturesService
    {
        public SignaturesService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

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
            return ConverterDataFile.SignaturesToDto(signatureEntities);
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
            return departments;
        }

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var signatureEntities = await unitOfWork.Session.Query<SignatureEntity>().
                                                     Where(signature => ids.Contains(signature.PersonId)).
                                                     ToListAsync();
            return ConverterDataFile.SignaturesToDto(signatureEntities);
        }

        /// <summary>
        /// Записать подписи в базу данных
        /// </summary>      
        public async Task<Unit> UploadSignatures(IList<SignatureDto> signaturesDto)
        {
            var signaturesEntity = ConverterDataFile.SignaturesFromDto(signaturesDto);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            foreach (var signatureEntity in signaturesEntity)
            {
                await unitOfWork.Session.SaveOrUpdateAsync(signatureEntity);
            }
            await unitOfWork.CommitAsync();
            return Unit.Value;
        }

        /// <summary>
        /// Удалить подписи в базе данных
        /// </summary>      
        public async Task<Unit> DeleteSignatures()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var signaturesEntity = await unitOfWork.Session.Query<SignatureEntity>().ToListAsync();
            foreach (var signatureEntity in signaturesEntity)
            {
                await unitOfWork.Session.DeleteAsync(signatureEntity);
            }
            await unitOfWork.CommitAsync();
            return Unit.Value;
        }

        /// <summary>
        /// Получить данные Microstation из базы данных
        /// </summary>      
        public async Task<MicrostationDataFileDto> GetMicrostationDataFile(string idDataFile)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var signatureMicrostationEntity = await unitOfWork.Session.LoadAsync<MicrostationDataFileEntity>(idDataFile);
            var signatureMicrostationDto = ConverterDataFile.SignatureMicrostationToDto(signatureMicrostationEntity);

            return signatureMicrostationDto;
        }

        /// <summary>
        /// Записать данные Microstation в базу данных
        /// </summary>      
        public async Task<Unit> UploadMicrostationDataFile(MicrostationDataFileDto microstationDataFileDto, string idDataFile)
        {
            var signatureMicrostationEntity = ConverterDataFile.MicrostationDataFileFromDto(microstationDataFileDto, idDataFile);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await unitOfWork.Session.SaveOrUpdateAsync(signatureMicrostationEntity);

            await unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}